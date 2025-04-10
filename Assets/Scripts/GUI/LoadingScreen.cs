using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class LoadingScreen : MonoBehaviour
    {
        public bool IsActive
        {
            set => gameObject.SetActive(value);
        }
        
        public float Progress
        {
            set => _progressFillImage.fillAmount = value;
        }
        
        [SerializeField] private Image _progressFillImage;
    }
}