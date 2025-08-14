using System;
using Framework;
using Framework.GameStateMachine;
using LocalMessages;
using Startup.GameStates;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameCore.Input
{
    public class DesktopInputSource : IInitializable, IDisposable, ITickable, IInputSource
    {
        [Inject] private readonly InputState _inputState;
        [Inject] private readonly GameStateMachine _gameStateMachine;
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        public bool JumpPressed { get; private set; }

        public void Initialize()
        {
            _inputState.RegisterInputSource(this);
        }

        public void Dispose()
        {
            _inputState.UnregisterInputSource(this);
        }

        public void Tick()
        {
            JumpPressed = UnityEngine.Input.GetKeyDown(KeyCode.Space);
            if (JumpPressed && !_gameStateMachine.IsInState<PlayGameStateData>())
                _gameStateMachine.SwitchToState<PlayGameStateData>();
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
                _localMessageBroker.TriggerEmpty<PlayerDieMessage>();
        }
    }
}