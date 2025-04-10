using UnityEngine;

namespace GameCore.Level
{
    public class LevelPart : MonoBehaviour
    {
        [field: SerializeField] public float HalfLength { get; private set; }
    }
}