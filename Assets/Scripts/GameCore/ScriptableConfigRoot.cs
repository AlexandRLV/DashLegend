using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "ScriptableConfigRoot", menuName = "Configs/Root")]
    public class ScriptableConfigRoot : ScriptableObject
    {
        [SerializeField] public ScriptableObject[] Configs;
    }
}