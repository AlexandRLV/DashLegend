using GameCore.Character;
using UnityEngine;
using VContainer;

namespace GameCore.Collectables
{
    public abstract class BaseCollectable : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _collectableComponents;
        
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;

        private bool _isMagnetActive;
        private Transform _magnetOrigin;
        private float _magnetSpeed;
        
        private void Awake()
        {
            Cleanup();
        }
        
        public void MoveToMagnet(Transform magnetOrigin, float moveSpeed)
        {
            _isMagnetActive = true;
            _magnetOrigin = magnetOrigin;
            _magnetSpeed = moveSpeed;

            foreach (var component in _collectableComponents)
            {
                component.enabled = false;
            }
        }

        private void Update()
        {
            if (!_isMagnetActive || _magnetOrigin == null)
                return;
            
            var direction = _magnetOrigin.position - transform.position;
            transform.position += direction.normalized * (_magnetSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponentInParent<PlayerCharacter>();
            if (character == null)
                return;

            Cleanup();
            _collectablesSpawner.CatchCollectable(this);
        }

        public void Cleanup()
        {
            _isMagnetActive = false;
            _magnetOrigin = null;
            _magnetSpeed = 0f;
            
            foreach (var component in _collectableComponents)
            {
                component.enabled = true;
            }
        }
    }
}