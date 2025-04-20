using System.Collections.Generic;
using Framework.DI;
using Framework.Extensions;
using Framework.Pools;
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

        [Inject] private readonly GameController _gameController;
        
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
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
                _spawnedParts.Remove(levelPart);
            }
            
            _spawnedParts.Clear();
        }

        private void Update()
        {
            if (!_enabled)
                return;

            _passedTime += Time.deltaTime;
            MoveSpawnedParts(out float furthestCoveredZ);
            CleanupPartsToDelete();
            SpawnNewParts(furthestCoveredZ);
        }

        private void MoveSpawnedParts(out float furthestCoveredZ)
        {
            float passedDistanceThisFrame = _gameController.RunSpeed * Time.deltaTime;
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
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
                _spawnedParts.Remove(levelPart);
            }
        }

        private void SpawnNewParts(float furthestCoveredZ)
        {
            while (furthestCoveredZ < _levelGeneratorConfig.CoverDistance)
            {
                var part = GetAvailableParts().GetRandomWithChance();
                var position = transform.position + Vector3.forward * (furthestCoveredZ + part.PartPrefab.HalfLength);
                var levelPart = PrefabMonoPool<LevelPart>.GetPrefabInstance(part.PartPrefab);

#if UNITY_EDITOR
                levelPart.transform.parent = transform;
#endif
                
                levelPart.transform.position = position;
                _spawnedParts.Add(levelPart);
                furthestCoveredZ += levelPart.HalfLength * 2f;
            }
        }

        private LevelPartContainer[] GetAvailableParts()
        {
            if (_passedTime < _levelGeneratorConfig.EmptyPartsSpawnTime)
                return _levelGeneratorConfig.EmptyParts;
            
            return _mode switch
            {
                LevelGeneratorMode.Game => _levelGeneratorConfig.GameLevelParts,
                LevelGeneratorMode.Menu => _levelGeneratorConfig.MenuLevelParts,
                _ => _levelGeneratorConfig.EmptyParts,
            };
        }
    }
}