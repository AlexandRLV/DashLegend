using GameCore;
using VContainer;

namespace Framework.GUI
{
    public class WindowWithPause : WindowBase
    {
        [Inject] private readonly GameTime _gameTime;

        private float _timeScale;
        
        protected virtual void Start()
        {
            _timeScale = _gameTime.TimeScale;
            _gameTime.TimeScale = 0f;
        }

        public override void Destroy()
        {
            _gameTime.TimeScale = _timeScale;
        }
    }
}