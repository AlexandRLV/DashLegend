using System;
using Framework;
using Framework.DI;
using Framework.GUI;
using Framework.MonoUpdate;
using GUI;
using LocalMessages;
using UnityEngine;

namespace GameCore
{
    public class GameController : IInitializable, IUpdatable, IDisposable, IMessageListener<PlayerDeadMessage>
    {
        public float RunSpeed => _gameConfig.DefaultRunSpeed * SpeedMultiplier;

        public float SpeedMultiplier = 1f;

        [Inject] private readonly GameTime _gameTime;
        [Inject] private readonly GameConfig _gameConfig;
        [Inject] private readonly MonoUpdater _monoUpdater;
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        private bool _inGame;
        private float _gameTimer;
        
        public void Initialize()
        {
            _localMessageBroker.Subscribe(this);
            _monoUpdater.AddUpdatable(this);
        }

        public void Dispose()
        {
            _localMessageBroker.Unsubscribe(this);
            _monoUpdater.RemoveUpdatable(this);
        }

        public void ResetInGameState(bool inGame)
        {
            _inGame = inGame;
            _gameTimer = 0f;
            _gameTime.TimeScale = 1f;
            SpeedMultiplier = 1f;
        }

        public void OnMessage(in PlayerDeadMessage message)
        {
            _windowsSystem.PushWindow<GameOverWindow>();
        }

        public void Update()
        {
            if (!_inGame)
                return;
            
            if (_gameTime.TimeScale >= _gameConfig.MaxTimeScale)
                return;

            _gameTimer += Time.deltaTime;
            if (_gameTimer < _gameConfig.TimeScaleIncreaseInterval)
                return;

            _gameTimer -= _gameConfig.TimeScaleIncreaseInterval;
            _gameTime.TimeScale += _gameConfig.TimeScaleIncreaseValue;
        }
    }
}