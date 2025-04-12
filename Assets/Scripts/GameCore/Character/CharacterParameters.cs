using UnityEngine;

namespace GameCore.Character
{
    [CreateAssetMenu(menuName = "Configs/Character Parameters")]
    public class CharacterParameters : ScriptableObject
    {
        [SerializeField] public float JumpHeight;
        [SerializeField] public float TotalJumpTime;
        [SerializeField] public float JumpUpTime;
        [SerializeField] public AnimationCurve JumpCurve;
    }
}