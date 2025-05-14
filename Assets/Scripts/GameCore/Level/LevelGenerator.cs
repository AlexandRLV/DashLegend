using System.Collections.Generic;
using Framework.DI;
using Framework.Extensions;
using Framework.Pools;
using GameCore.Collectables;
using UnityEngine;

namespace GameCore.Level
{
    public enum LevelGeneratorMode
    {
        Menu,
        Game,
    }
    
    public class LevelGenerator : MonoBehaviour
    {
        public float PassedDistance => _passedDistance;

        [SerializeField] private LevelGeneratorConfig _levelGeneratorConfig;

        [Inject] private readonly GameTime _gameTime;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly LevelDecorSpawner _levelDecorSpawner;
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;
        
        private LevelGeneratorMode _mode;

        private bool _enabled;
        private float _passedDistance;
        private float _passedTime;

        private readonly List<LevelPart> _spawnedParts = new();
        private readonly List<LevelPart> _partsToDestroy = new();

        public void StartSpawn(LevelGeneratorMode mode)
        {
            _mode = mode;
            _enabled = true;
            _passedDistance = 0f;
            _passedTime = 0f;
        }

        public void Clear()
        {
            _enabled = false;
            _partsToDestroy.Clear();
            _partsToDestroy.AddRange(_spawnedParts);
            
            foreach (var levelPart in _partsToDestroy)
            {
                _collectablesSpawner.ProcessDespawnPart(levelPart);
                if (levelPart.DecorPart != null)
                    _levelDecorSpawner.ProcessDespawnPart(levelPart.DecorPart);
                
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
                _spawnedParts.Remove(levelPart);
            }
            
            _spawnedParts.Clear();
        }

        private void Update()
        {
            if (!_enabled)
                return;

            _passedTime += _gameTime.DeltaTime;
            MoveSpawnedParts(out float furthestCoveredZ);
            CleanupPartsToDelete();
            SpawnNewParts(furthestCoveredZ);
        }

        private void MoveSpawnedParts(out float furthestCoveredZ)
        {
            float passedDistanceThisFrame = _gameController.RunSpeed * _gameTime.DeltaTime;
            _passedDistance += passedDistanceThisFrame;
            
            _partsToDestroy.Clear();
            furthestCoveredZ = -_levelGeneratorConfig.DestroyDistance;
            foreach (var levelPart in _spawnedParts)
            {
                levelPart.transform.position -= Vector3.forward * passedDistanceThisFrame;
                float maxPartZ = levelPart.transform.position.z + levelPart.HalfLength;
                if (maxPartZ > furthestCoveredZ)
                    furthestCoveredZ = maxPartZ;

                if (maxPartZ < -_levelGeneratorConfig.DestroyDistance)
                    _partsToDestroy.Add(levelPart);
            }
        }

        private void CleanupPartsToDelete()
        {
            foreach (var levelPart in _partsToDestroy)
            {
                _collectablesSpawner.ProcessDespawnPart(levelPart);
                if (levelPart.DecorPart != null)
                    _levelDecorSpawner.ProcessDespawnPart(levelPart.DecorPart);
                
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
                _spawnedParts.Remove(levelPart);
            }
        }

        private void SpawnNewParts(float furthestCoveredZ)
        {
            const int maxTries = 1000;
            int tries = 0;
            while (furthestCoveredZ < _levelGeneratorConfig.CoverDistance && tries < maxTries)
            {
                tries++;
                var part = GetAvailableParts().GetRandomWithChance();
                if (part.PartPrefab.HalfLength <= 0f)
                    continue;
                
                var position = transform.position + Vector3.forward * (furthestCoveredZ + part.PartPrefab.HalfLength);
                var levelPart = PrefabMonoPool<LevelPart>.GetPrefabInstance(part.PartPrefab);

#if UNITY_EDITOR
                levelPart.transform.parent = transform;
#endif
                
                levelPart.transform.position = position;
                _spawnedParts.Add(levelPart);
                furthestCoveredZ += levelPart.HalfLength * 2f;
                
                _collectablesSpawner.ProcessSpawnPart(levelPart);
                if (levelPart.DecorPart != null)
                    _levelDecorSpawner.ProcessSpawnPart(levelPart.DecorPart);
            }

            if (tries >= maxTries)
            {
                Debug.LogError("[LevelGenerator] In 1000 tries cover distance didn't covered - wrong prefabs configuration");
                Clear();
            }
        }

        private LevelPartContainer[] GetAvailableParts()
        {
            if (_passedTime < _levelGeneratorConfig.EmptyPartsSpawnTime)
                return _levelGeneratorConfig.EmptyConfig.Parts;
            
            return _mode switch
            {
                LevelGeneratorMode.Game => _levelGeneratorConfig.GameLevelConfig.Parts,
                LevelGeneratorMode.Menu => _levelGeneratorConfig.MenuLevelConfig.Parts,
                _ => _levelGeneratorConfig.EmptyConfig.Parts,
            };
        }
    }
}