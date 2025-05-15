using UnityEngine;

namespace GameCore.Level.Props
{
    public class DecorPlace : MonoBehaviour
    {
        [SerializeField] public float Radius;
        [SerializeField] public int MaxCount;
        [SerializeField] [Range(0f, 1f)] public float ItemChance;
        [SerializeField] [Range(0f, 1f)] public float TotalChance;
        [SerializeField] public DecorType DecorType;
    }
}