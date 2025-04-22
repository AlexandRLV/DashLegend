using Framework;
using Framework.DI;
using GameCore.Character;
using LocalMessages;
using UnityEngine;

namespace GameCore.Collectables
{
    public abstract class BaseCollectable : MonoBehaviour
    {
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>() != null)
                return;

            var message = new CatchCollectableMessage
            {
                Value = this
            };
            _localMessageBroker.Trigger(message);
        }
    }
}