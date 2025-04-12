using Framework.DI;

namespace GameCore
{
    public class RuntimeGameState
    {
        public float RunSpeed => _gameConfig.DefaultRunSpeed * SpeedMultiplier;
        
        public float SpeedMultiplier = 1f;

        [Inject] private readonly GameConfig _gameConfig;
    }
}