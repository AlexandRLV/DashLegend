using System;
using Framework;
using Framework.DI;
using Framework.GUI;
using GUI;
using LocalMessages;

namespace GameCore
{
    public class GameController : IDisposable, IMessageListener<PlayerDeadMessage>
    {
        public float RunSpeed => _gameConfig.DefaultRunSpeed * SpeedMultiplier;
        
        public float SpeedMultiplier = 1f;

        [Inject] private readonly GameConfig _gameConfig;
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        public void Initialize()
        {
            _localMessageBroker.Subscribe(this);
        }

        public void Dispose()
        {
            _localMessageBroker.Unsubscribe(this);
        }

        public void OnMessage(in PlayerDeadMessage message)
        {
            _windowsSystem.PushWindow<GameOverWindow>();
        }
    }
}