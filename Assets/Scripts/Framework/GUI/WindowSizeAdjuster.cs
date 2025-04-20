using UnityEngine;

namespace Framework.GUI
{
    public class WindowSizeAdjuster : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.offsetMin = Vector2.zero;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }
#endif
    }
}