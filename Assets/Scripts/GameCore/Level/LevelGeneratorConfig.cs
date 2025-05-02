using UnityEngine;

namespace GameCore.Level
{
    [CreateAssetMenu(menuName = "Configs/Level Generator")]
    public class LevelGeneratorConfig : ScriptableObject
    {
        [SerializeField] public float EmptyPartsSpawnTime;
        [SerializeField] public float CoverDistance;
        [SerializeField] public float DestroyDistance;
        [SerializeField] public LevelPartsConfig EmptyConfig;
        [SerializeField] public LevelPartsConfig GameLevelConfig;
        [SerializeField] public LevelPartsConfig MenuLevelConfig;
    }
}