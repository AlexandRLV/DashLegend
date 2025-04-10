using UnityEngine;

namespace GameCore.Level
{
    [CreateAssetMenu(menuName = "Configs/Level Parts")]
    public class LevelPartsConfig : ScriptableObject
    {
        [SerializeField] public LevelPart[] LevelParts;
    }
}