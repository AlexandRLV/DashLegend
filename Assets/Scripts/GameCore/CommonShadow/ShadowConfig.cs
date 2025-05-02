using UnityEngine;

namespace GameCore.CommonShadow
{
    [CreateAssetMenu(fileName = "ShadowConfig", menuName = "Configs/Shadow")]
    public class ShadowConfig : ScriptableObject
    {
        [SerializeField] public GameObject ShadowPrefab;
        [SerializeField] public float FloorHeightOffset;
    }
}