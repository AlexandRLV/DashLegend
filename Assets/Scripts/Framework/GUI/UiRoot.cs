using UnityEngine;

namespace Framework.GUI
{
    public class UiRoot : MonoBehaviour
    {
        [field: SerializeField] public RectTransform WindowsParent { get; private set; }
    }
}