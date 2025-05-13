using System;
using UnityEngine;

namespace Framework.Sounds
{
    [Serializable]
    public class SoundContainer
    {
        [SerializeField] public SoundType Type;
        [SerializeField] public AudioClip Clip;
    }
    
    [Serializable]
    public class MusicContainer
    {
        [SerializeField] public MusicType Type;
        [SerializeField] public AudioClip[] Clips;
    }
    
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/Sounds")]
    public class SoundsConfig : ScriptableObject
    {
        [SerializeField] public float MusicTransitionTime;
        [SerializeField] public float MusicTransitionOffset;
        [SerializeField] public SoundContainer[] Sounds;
        [SerializeField] public MusicContainer[] Music;
    }
}