using Framework.DI;
using Framework.Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.GUI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundProvider : MonoBehaviour
    {
        [SerializeField] private SoundType _soundType = SoundType.Click;
        [SerializeField] private Button _button;

        [Inject] private readonly SoundSystem _soundSystem;

        private void Start()
        {
            GameContainer.Current.InjectToInstance(this);
            _button.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            _soundSystem.PlaySound(_soundType);
        }

        private void OnValidate()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }
    }
}