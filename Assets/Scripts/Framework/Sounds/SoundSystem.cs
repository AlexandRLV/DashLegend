using System.Collections.Generic;
using Framework.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace Framework.Sounds
{
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _soundsMixerGroup;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private SoundsConfig _config;
        [SerializeField] private AudioSource _soundsSource;
        [SerializeField] private AudioSource _musicFirstSource;
        [SerializeField] private AudioSource _musicSecondSource;

        private MusicType _currentMusicType;
        private float _currentMusicTimer;

        private bool _fadeInProgress;
        private float _fadeStartVolume;
        private float _fadeTimer;
        
        private AudioSource _currentMusicSource;
        private AudioSource _nextMusicSource;
        
        private Dictionary<SoundType, AudioClip> _sounds;
        private Dictionary<MusicType, AudioClip[]> _music;

        public void Awake()
        {
            _sounds = new Dictionary<SoundType, AudioClip>();
            _music = new Dictionary<MusicType, AudioClip[]>();

            foreach (var soundContainer in _config.Sounds)
            {
                _sounds[soundContainer.Type] = soundContainer.Clip;
            }

            foreach (var musicContainer in _config.Music)
            {
                _music[musicContainer.Type] = musicContainer.Clips;
            }

            _currentMusicSource = _musicFirstSource;
            _nextMusicSource = _musicSecondSource;
        }

        private void Update()
        {
            UpdateFade();
            if (_currentMusicType == MusicType.None)
                return;

            _currentMusicTimer -= Time.deltaTime;
            if (_currentMusicTimer > _config.MusicTransitionOffset)
                return;
            
            var clips = _music[_currentMusicType];
            if (clips.Length < 2) return;

            var nextClip = clips.GetRandom();
            FadeMusic(nextClip);
        }

        private void UpdateFade()
        {
            if (!_fadeInProgress)
                return;

            _fadeTimer += Time.deltaTime;
            if (_fadeTimer >= _config.MusicTransitionTime)
            {
                _nextMusicSource.volume = 1f;
                _currentMusicSource.volume = 0f;
                (_currentMusicSource, _nextMusicSource) = (_nextMusicSource, _currentMusicSource);
                return;
            }
            
            float t = _fadeTimer / _config.MusicTransitionTime;
            _currentMusicSource.volume = Mathf.Lerp(_fadeStartVolume, 0f, t);
            _nextMusicSource.volume = Mathf.Lerp(0f, 1f, t);
            _fadeInProgress = false;
        }

        public void PlaySound(SoundType soundType)
        {
            if (!_sounds.TryGetValue(soundType, out var clip))
            {
                Debug.LogError($"[SoundsSystem] Trying to play not registered sound type: {soundType}");
                return;
            }
            
            _soundsSource.PlayOneShot(clip);
        }

        public void PlayMusic(MusicType musicType)
        {
            if (!_music.TryGetValue(musicType, out var clips))
            {
                Debug.LogError($"[SoundsSystem] Trying to play not registered music type: {musicType}");
                return;
            }
            
            var nextClip = clips.GetRandom();
            FadeMusic(nextClip);
        }

        private void FadeMusic(AudioClip next)
        {
            _fadeStartVolume = _currentMusicSource.volume;
            _fadeInProgress = true;
            _fadeTimer = 0f;

            _nextMusicSource.Stop();
            _nextMusicSource.clip = next;
            _nextMusicSource.volume = 0f;
            _nextMusicSource.Play();

            _currentMusicTimer = next.length;
        }
    }
}