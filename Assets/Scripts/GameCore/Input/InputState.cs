using System.Collections.Generic;
using VContainer.Unity;

namespace GameCore.Input
{
    public class InputState : ITickable
    {
        public bool JumpPressed;

        private readonly List<IInputSource> _inputSources = new();

        public void RegisterInputSource(IInputSource inputSource)
        {
            if (!_inputSources.Contains(inputSource))
                _inputSources.Add(inputSource);
        }

        public void UnregisterInputSource(IInputSource inputSource)
        {
            _inputSources.Remove(inputSource);
        }

        public void Tick()
        {
            Reset();
            
            foreach (var inputSource in _inputSources)
            {
                JumpPressed = JumpPressed || inputSource.JumpPressed;
            }
        }

        private void Reset()
        {
            JumpPressed = false;
        }
    }
}