using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(menuName = "Configs/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] public float DefaultRunSpeed;
    }
}