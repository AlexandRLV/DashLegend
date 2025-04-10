using System.Collections.Generic;
using Framework.DI;
using Framework.Extensions;
using Framework.Pools;
using UnityEngine;

namespace GameCore.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private float _coverDistance;
        [SerializeField] private float _destroyDistance;

        [Inject] private readonly RuntimeGameState _runtimeGameState;
        
        private bool _enabled;
        private LevelPartsConfig _availableLevelParts;

        private readonly List<LevelPart> _spawnedParts = new();
        private readonly List<LevelPart> _partsToDestroy = new();

        public void StartSpawn(LevelPartsConfig availableParts)
        {
            if (availableParts == null || availableParts.LevelParts == null || availableParts.LevelParts.Length == 0)
            {
                Debug.LogError("[LevelGenerator] Available parts config is null or empty");
                return;
            }
            
            _availableLevelParts = availableParts;
            _enabled = true;
        }

        private void Update()
        {
            if (!_enabled)
                return;

            _partsToDestroy.Clear();
            float furthestCoveredZ = 0f;
            foreach (var levelPart in _spawnedParts)
            {
                levelPart.transform.position -= Vector3.forward * (_runtimeGameState.RunSpeed * Time.deltaTime);
                float maxPartZ = levelPart.transform.position.z + levelPart.HalfLength;
                if (maxPartZ > furthestCoveredZ)
                    furthestCoveredZ = maxPartZ;

                if (maxPartZ < _destroyDistance)
                    _partsToDestroy.Add(levelPart);
            }

            foreach (var levelPart in _partsToDestroy)
            {
                PrefabMonoPool<LevelPart>.ReturnInstance(levelPart);
                _spawnedParts.Remove(levelPart);
            }

            while (furthestCoveredZ < _coverDistance)
            {
                var partPrefab = _availableLevelParts.LevelParts.GetRandom();
                var position = transform.position + Vector3.forward * (furthestCoveredZ + partPrefab.HalfLength);
                var levelPart = PrefabMonoPool<LevelPart>.GetPrefabInstance(partPrefab);
                levelPart.transform.position = position;
                _spawnedParts.Add(levelPart);
                furthestCoveredZ += levelPart.HalfLength * 2f;
            }
        }
    }
}