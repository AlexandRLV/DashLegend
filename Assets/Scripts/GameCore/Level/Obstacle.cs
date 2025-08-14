using GameCore.Boosters;
using GameCore.Character;
using UnityEngine;
using VContainer;

namespace GameCore.Level
{
    public class Obstacle : MonoBehaviour
    {
        [Inject] private readonly BoostersService _boostersService;
        
        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponentInParent<PlayerCharacter>();
            if (character == null)
                return;
            
            if (_boostersService.IsBoosterActive(BoosterType.Shield))
            {
                gameObject.SetActive(false);
                // TODO: play shield hit effect
                return;
            }
            
            character.ProcessObstacleHit();
        }
    }
}