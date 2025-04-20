using System;
using Framework.DI;
using Framework.GameStateMachine;
using Framework.MonoUpdate;
using Startup.GameStates;
using UnityEngine;

namespace GameCore.Input
{
    public class DesktopInputSource : IDisposable, IUpdatable, IInputSource
    {
        [Inject] private readonly MonoUpdater _monoUpdater;
        [Inject] private readonly InputState _inputState;
        [Inject] private readonly GameStateMachine _gameStateMachine;

        public bool JumpPressed { get; private set; }

        public void Initialize()
        {
            _monoUpdater.AddUpdatable(this);
            _inputState.RegisterInputSource(this);
        }

        public void Dispose()
        {
            _monoUpdater.RemoveUpdatable(this);
            _inputState.UnregisterInputSource(this);
        }

        public void Update()
        {
            JumpPressed = UnityEngine.Input.GetKeyDown(KeyCode.Space);
            if (JumpPressed && !_gameStateMachine.IsInState<PlayGameStateData>())
                _gameStateMachine.SwitchToState<PlayGameStateData>();
        }
    }
}