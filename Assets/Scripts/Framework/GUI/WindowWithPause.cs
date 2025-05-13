using Framework.DI;
using GameCore;

namespace Framework.GUI
{
    public class WindowWithPause : WindowBase
    {
        [Inject] private readonly GameTime _gameTime;

        private float _timeScale;
        
        protected override void Start()
        {
            base.Start();
            _timeScale = _gameTime.TimeScale;
            _gameTime.TimeScale = 0f;
        }

        public override void Destroy()
        {
            base.Destroy();
            _gameTime.TimeScale = _timeScale;
        }
    }
}