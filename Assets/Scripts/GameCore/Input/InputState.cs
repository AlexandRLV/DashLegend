using System;
using System.Collections.Generic;
using Framework.DI;
using Framework.MonoUpdate;

namespace GameCore.Input
{
    public class InputState : IInitializable, IUpdatable, IDisposable
    {
        public bool JumpPressed;

        [Inject] private readonly MonoUpdater _monoUpdater;
        
        private readonly List<IInputSource> _inputSources = new();
        
        public void Initialize()
        {
            _monoUpdater.AddUpdatable(this);
        }

        public void RegisterInputSource(IInputSource inputSource)
        {
            if (!_inputSources.Contains(inputSource))
                _inputSources.Add(inputSource);
        }

        public void UnregisterInputSource(IInputSource inputSource)
        {
            _inputSources.Remove(inputSource);
        }

        public void Update()
        {
            Reset();
            
            foreach (var inputSource in _inputSources)
            {
                JumpPressed = JumpPressed || inputSource.JumpPressed;
            }
        }

        public void Dispose()
        {
            _monoUpdater.RemoveUpdatable(this);
        }

        private void Reset()
        {
            JumpPressed = false;
        }
    }
}