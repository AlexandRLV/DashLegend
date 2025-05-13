using Framework.DI;
using UnityEngine;

namespace Framework
{
    public class SettingsProvider
    {
        private const string SoundVolumeKey = "SettingsParameterSoundsVolume";
        private const string MusicVolumeKey = "SettingsParameterMusicVolume";
        private const string SoundOnKey = "SettingsParameterSoundsOn";
        private const string MusicOnKey = "SettingsParameterMusicOn";
        
        public float SoundsVolume;
        public float MusicVolume;
        public bool SoundsOn;
        public bool MusicOn;

        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        public void TriggerChange()
        {
            _localMessageBroker.TriggerEmpty<SettingsChangedMessage>();
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, SoundsVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
            PlayerPrefs.SetInt(SoundOnKey, SoundsOn ? 1 : 0);
            PlayerPrefs.SetInt(MusicOnKey, MusicOn ? 1 : 0);
            
            TriggerChange();
        }

        public void Load()
        {
            SoundsVolume = PlayerPrefsUtils.GetKeyOrDefault(SoundVolumeKey, 1f);
            MusicVolume = PlayerPrefsUtils.GetKeyOrDefault(MusicVolumeKey, 1f);
            SoundsOn = PlayerPrefsUtils.GetKeyOrDefault(SoundOnKey, 1) == 1;
            MusicOn = PlayerPrefsUtils.GetKeyOrDefault(MusicOnKey, 1) == 1;
        }
    }
}