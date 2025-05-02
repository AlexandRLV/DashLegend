using Framework.DI;
using Framework.Extensions;
using UnityEngine;

namespace GameCore.Collectables
{
    public class Floater : MonoBehaviour
    {
        [SerializeField] private bool _useWorldPosition;
        [SerializeField] private float _period;
        [SerializeField] private float _minYOffset;
        [SerializeField] private float _maxYOffset;
        [SerializeField] private AnimationCurve _curve;

        [Inject] private readonly GameTime _gameTime;

        private float _startY;
        private float _timer;

        private void Start()
        {
            GameContainer.Current.InjectToInstance(this);
            _startY = _useWorldPosition ? transform.position.y : transform.localPosition.y;
            _timer = 0f;
        }

        private void Update()
        {
            _timer += _gameTime.DeltaTime;
            _timer %= _period;

            float t = _timer / _period;
            float yOffset = Mathf.Lerp(_minYOffset, _maxYOffset, _curve.Evaluate(t));
            
            if (_useWorldPosition)
                transform.SetYPosition(_startY + yOffset);
            else
                transform.SetYLocalPosition(_startY + yOffset);
        }
    }
}