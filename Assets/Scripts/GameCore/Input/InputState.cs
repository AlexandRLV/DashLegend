using System;
using System.Collections.Generic;
using Framework.DI;
using Framework.MonoUpdate;

namespace GameCore.Input
{
    public class InputState : IUpdatable, IDisposable
    {
        public bool JumpPressed;

        [Inject] private readonly MonoUpdater _monoUpdater;
        
        private List<IInputSource> _inputSources;
        
        public void Initialize(List<IInputSource> inputSources)
        {
            _inputSources = inputSources;
            _monoUpdater.AddUpdatable(this);
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