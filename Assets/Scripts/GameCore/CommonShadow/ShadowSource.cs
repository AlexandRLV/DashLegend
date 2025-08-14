using UnityEngine;
using VContainer;

namespace GameCore.CommonShadow
{
    public class ShadowSource : MonoBehaviour
    {
        [SerializeField] private float _shadowScale = 1f;
        
        [Inject] private readonly ShadowController _shadowController;
        
        private void Start()
        {
            _shadowController.AddShadow(transform, _shadowScale);
        }

        private void OnDestroy()
        {
            _shadowController.RemoveShadow(transform);
        }
    }
}