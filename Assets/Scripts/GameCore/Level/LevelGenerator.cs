using System.Collections.Generic;
using Framework.DI;
using Framework.Extensions;
using Framework.Pools;
using GameCore.Collectables;
using GameCore.Level.Props;
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

        [SerializeField] private Transform _mainLineOrigin;
        [SerializeField] private Transform _leftSideDecorLineOrigin;
        [SerializeField] private Transform _rightSideDecorLineOrigin;
        [SerializeField] private LevelGeneratorConfig _levelGeneratorConfig;

        [Inject] private readonly GameTime _gameTime;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly LevelPropsSpawner _levelPropsSpawner;
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;
        
        private LevelGeneratorMode _mode;

        private bool _enabled;
        private float _passedDistance;
        private float _passedDistanceThisFrame;
        private float _passedTime;

        private readonly List<LevelPart> _mainRunLine = new();
        private readonly List<LevelPart> _leftSideDecorLine = new();
        private readonly List<LevelPart> _rightSideDecorLine = new();
        private readonly List<LevelPart> _partsToDestroy = new();

        public void StartSpawn(LevelGeneratorMode mode)
        {
            _mode = mode;
            _enabled = true;
            _passedDistance = 0f;
            _passedDistanceThisFrame = 0f;
            _passedTime = 0f;
        }

        public void Clear()
        {
            _enabled = false;
            _partsToDestroy.Clear();
            _partsToDestroy.AddRange(_mainRunLine);
            _partsToDestroy.AddRange(_leftSideDecorLine);
            _partsToDestroy.AddRange(_rightSideDecorLine);

            CleanupPartsToDelete();
            
            _mainRunLine.Clear();
            _leftSideDecorLine.Clear();
            _rightSideDecorLine.Clear();
        }

        private void Update()
        {
            if (!_enabled)
                return;

            _passedTime += _gameTime.DeltaTime;
            _partsToDestroy.Clear();
            
            _passedDistanceThisFrame = _gameController.RunSpeed * _gameTime.DeltaTime;
            _passedDistance += _passedDistanceThisFrame;
            
            ProcessPartsList(_mainLineOrigin, _mainRunLine, GetMainLineAvailableParts());
            ProcessPartsList(_leftSideDecorLineOrigin, _leftSideDecorLine, _levelGeneratorConfig.SideDecorConfig.Parts);
            ProcessPartsList(_rightSideDecorLineOrigin, _rightSideDecorLine, _levelGeneratorConfig.SideDecorConfig.Parts);
            
            CleanupPartsToDelete();
        }

        private void ProcessPartsList(Transform lineOrigin, List<LevelPart> spawnedParts, LevelPartContainer[] availableParts)
        {
            MoveSpawnedParts(spawnedParts, out float furthestCoveredZ);
            SpawnNewParts(lineOrigin, spawnedParts, availableParts, furthestCoveredZ);
        }

        private void MoveSpawnedParts(List<LevelPart> spawnedParts, out float furthestCoveredZ)
        {
            furthestCoveredZ = -_levelGeneratorConfig.DestroyDistance;
            if (spawnedParts.Count == 0)
                return;
            
            for (int i = spawnedParts.Count - 1; i >= 0; i--)
            {
                var levelPart = spawnedParts[i];
                levelPart.transform.position -= Vector3.forward * _passedDistanceThisFrame;
                float maxPartZ = levelPart.transform.position.z + levelPart.HalfLength;
                if (maxPartZ > furthestCoveredZ)
                    furthestCoveredZ = maxPartZ;

                if (maxPartZ < -_levelGeneratorConfig.DestroyDistance)
                {
                    _partsToDestroy.Add(levelPart);
                    spawnedParts.RemoveAt(i);
                }
            }
        }

        private void SpawnNewParts(Transform lineOrigin, List<LevelPart> spawnedParts, LevelPartContainer[] availableParts, float furthestCoveredZ)
        {
            if (availableParts == null || availableParts.Length == 0)
                return;
            
            const int maxTries = 1000;
            int tries = 0;
            while (furthestCoveredZ < _levelGeneratorConfig.CoverDistance && tries < maxTries)
            {
                tries++;
                var part = availableParts.GetRandomWithChance();
                if (part.PartPrefab.HalfLength <= 0f)
                    continue;
                
                var position = lineOrigin.position + Vector3.forward * (furthestCoveredZ + part.PartPrefab.HalfLength);
                var rotation = lineOrigin.rotation;
                var levelPart = PrefabMonoPool<LevelPart>.GetPrefabInstance(part.PartPrefab);

#if UNITY_EDITOR
                levelPart.transform.parent = lineOrigin;
#endif
                
                levelPart.transform.SetPositionAndRotation(position, rotation);
                spawnedParts.Add(levelPart);
                furthestCoveredZ += levelPart.HalfLength * 2f;
                
                _collectablesSpawner.ProcessSpawnPart(levelPart);
                if (levelPart.PropsPart != null)
                    _levelPropsSpawner.ProcessSpawnPart(levelPart.PropsPart);
            }

            if (tries >= maxTries)
            {
                Debug.LogError("[LevelGenerator] In 1000 tries cover distance didn't covered - wrong prefabs configuration");
                Clear();
            }
        }

        private void CleanupPartsToDelete()
        {
            foreach (var levelPart in _partsToDestroy)
            {
                _collectablesSpawner.ProcessDespawnPart(levelPart);
                if (levelPart.PropsPart != null)
                    _levelPropsSpawner.ProcessDespawnPart(levelPart.PropsPart);
                
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
            }
        }

        private LevelPartContainer[] GetMainLineAvailableParts()
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