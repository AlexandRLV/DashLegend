using Framework.DI;
using GameCore.Boosters;
using GameCore.Character;
using UnityEngine;

namespace GameCore.Level
{
    public class Obstacle : MonoBehaviour
    {
        [Inject] private readonly BoostersService _boostersService;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerCharacter character))
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