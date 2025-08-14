using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class ActiveBoosterItem : MonoBehaviour
    {
        [SerializeField] private Image _boosterImage;
        [SerializeField] private TMP_Text _timeText;

        private int _lastShownSeconds;
        private float _remainingTime;
        
        public void Init(Sprite sprite, float remainingTime)
        {
            _boosterImage.sprite = sprite;
            _remainingTime = remainingTime;
            _lastShownSeconds = 0;
            UpdateTime();
        }

        public void SetTime(float remainingTime)
        {
            _remainingTime = remainingTime;
            _lastShownSeconds = 0;
            UpdateTime();
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime >= 0f)
                UpdateTime();
        }

        private void UpdateTime()
        {
            int seconds = Mathf.FloorToInt(_remainingTime);
            seconds = Mathf.Max(0, seconds);
            if (seconds == _lastShownSeconds)
                return;
            
            _lastShownSeconds = seconds;
            _timeText.text = seconds.ToString();
        }
    }
}