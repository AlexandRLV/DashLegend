using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCollisionEventReceiver : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _character;

        private void OnTriggerEnter(Collider other)
        {
            _character.OnTrigger(other);
        }
    }
}