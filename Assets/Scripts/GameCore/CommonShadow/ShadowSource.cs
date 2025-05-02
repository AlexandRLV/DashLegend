using Framework.DI;
using UnityEngine;

namespace GameCore.CommonShadow
{
    public class ShadowSource : MonoBehaviour
    {
        [SerializeField] private float _shadowScale = 1f;
        
        [Inject] private readonly ShadowController _shadowController;
        
        private void OnEnable()
        {
            GameContainer.Current.InjectToInstance(this);
            _shadowController.AddShadow(transform, _shadowScale);
        }

        private void OnDisable()
        {
            _shadowController.RemoveShadow(transform);
        }
    }
}