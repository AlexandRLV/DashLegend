using Framework.DI;
using GameCore.Character;
using UnityEngine;

namespace GameCore.Collectables
{
    public abstract class BaseCollectable : MonoBehaviour
    {
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
            if (other.GetComponent<PlayerCharacter>() != null)
                return;

            Cleanup();
            _collectablesSpawner.CatchCollectable(this);
        }

        public void Cleanup()
        {
            _isMagnetActive = false;
            _magnetOrigin = null;
            _magnetSpeed = 0f;
        }
    }
}