using GameCore.Character;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(menuName = "Configs/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] public float DefaultRunSpeed;
        [SerializeField] public float TimeScaleIncreaseInterval;
        [SerializeField] public float TimeScaleIncreaseValue;
        [SerializeField] public float MaxTimeScale;
        
        [SerializeField] public float GroundYOffset;
        [SerializeField] public PlayerCharacter PlayerCharacterPrefab;
        [SerializeField] public CharacterVisuals CharacterVisualsPrefab;
    }
}