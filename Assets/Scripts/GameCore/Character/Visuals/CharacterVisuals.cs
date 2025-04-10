using UnityEngine;

namespace GameCore.Character
{
    public class CharacterVisuals : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private AnimationType _currentAnimation;
        
        public void PlayAnimation(AnimationType animationType)
        {
            
        }
    }
}