using Framework.DI;
using GameCore.Character;
using UnityEngine;

namespace GameCore.Collectables
{
    public abstract class BaseCollectable : MonoBehaviour
    {
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>() != null)
                return;

            _collectablesSpawner.CatchCollectable(this);
        }
    }
}