using System.Collections.Generic;
using GameCore.Skins;
using UnityEngine;

namespace GameCore.Character
{
    public class CharacterVisuals : MonoBehaviour
    {
        private static readonly Dictionary<AnimationType, int> _animationHashes = new Dictionary<AnimationType, int>
        {
            { AnimationType.Run, Animator.StringToHash("Run") },
            { AnimationType.Jump, Animator.StringToHash("Jump") },
            { AnimationType.Fall, Animator.StringToHash("Fall") },
            { AnimationType.Die, Animator.StringToHash("Die") },
        };
        
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterSkin[] _skins;

        private AnimationType _currentAnimation;
        
        public void PlayAnimation(AnimationType animationType)
        {
            if (!_animationHashes.TryGetValue(animationType, out var stateHash))
                return;
            
            if (_currentAnimation == animationType)
                return;

            _currentAnimation = animationType;
            _animator.CrossFade(stateHash, 0.2f, 0);
        }

        public void SetSkin(SkinType type)
        {
            bool hasActiveSkin = false;
            foreach (var skin in _skins)
            {
                bool isActive = skin.Type == type;
                skin.gameObject.SetActive(isActive);
                if (isActive)
                    hasActiveSkin = true;
            }
            
            if (!hasActiveSkin)
                Debug.LogError($"[CharacterVisuals] Can't find skin to activate {type}");
        }
    }
}