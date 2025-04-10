using UnityEngine;

namespace GameCore.Character
{
    [CreateAssetMenu(menuName = "Configs/Character Parameters")]
    public class CharacterParameters : ScriptableObject
    {
        [SerializeField] public float JumpHeight;
        [SerializeField] public float JumpTime;
        [SerializeField] public AnimationCurve JumpCurve;
    }
}