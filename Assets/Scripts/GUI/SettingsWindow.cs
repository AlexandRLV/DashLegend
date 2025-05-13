using Framework;
using Framework.DI;
using Framework.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class SettingsWindow : WindowWithPause
    {
        [SerializeField] private Button[] _closeButtons;
        
        [Header("Sounds")]
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Button _soundOnOffButton;
        [SerializeField] private GameObject _soundOnState;
        [SerializeField] private GameObject _soundOffState;
        
        [Header("Music")]
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Button _musicOnOffButton;
        [SerializeField] private GameObject _musicOnState;
        [SerializeField] private GameObject _musicOffState;

        [Inject] private readonly SettingsProvider _settingsProvider;

        protected override void Start()
        {
            base.Start();
            foreach (var closeButton in _closeButtons)
            {
                closeButton.onClick.AddListener(OnClosePressed);
            }
            
            _soundVolumeSlider.value = _settingsProvider.SoundsOn ? _settingsProvider.SoundsVolume : 0f;
            _musicVolumeSlider.value = _settingsProvider.MusicOn ? _settingsProvider.MusicVolume : 0f;
            
            _soundOnState.SetActive(_settingsProvider.SoundsOn);
            _soundOffState.SetActive(!_settingsProvider.SoundsOn);
            _musicOnState.SetActive(_settingsProvider.MusicOn);
            _musicOffState.SetActive(!_settingsProvider.MusicOn);
            
            _soundOnOffButton.onClick.AddListener(SoundOnOffPressed);
            _musicOnOffButton.onClick.AddListener(MusicOnOffPressed);
            
            _soundVolumeSlider.onValueChanged.AddListener(OnSoundValueChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
        }

        private void OnClosePressed()
        {
            _settingsProvider.Save();
            Close();
        }

        private void SoundOnOffPressed()
        {
            _settingsProvider.SoundsOn = !_settingsProvider.SoundsOn;
            _soundVolumeSlider.SetValueWithoutNotify(_settingsProvider.SoundsOn ? _settingsProvider.SoundsVolume : 0f);
            _soundOnState.SetActive(_settingsProvider.SoundsOn);
            _soundOffState.SetActive(!_settingsProvider.SoundsOn);
            _settingsProvider.TriggerChange();
        }

        private void MusicOnOffPressed()
        {
            _settingsProvider.MusicOn = !_settingsProvider.MusicOn;
            _musicVolumeSlider.SetValueWithoutNotify(_settingsProvider.MusicOn ? _settingsProvider.MusicVolume : 0f);
            _musicOnState.SetActive(_settingsProvider.MusicOn);
            _musicOffState.SetActive(!_settingsProvider.MusicOn);
            _settingsProvider.TriggerChange();
        }

        private void OnSoundValueChanged(float value)
        {
            _soundOnState.SetActive(value > 0);
            _soundOffState.SetActive(value <= 0);
            _settingsProvider.SoundsVolume = value;
            _settingsProvider.TriggerChange();
        }

        private void OnMusicValueChanged(float value)
        {
            _musicOnState.SetActive(value > 0);
            _musicOffState.SetActive(value <= 0);
            _settingsProvider.MusicVolume = value;
            _settingsProvider.TriggerChange();
        }
    }
}