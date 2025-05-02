using System;
using Framework.DI;
using UnityEngine;

namespace GameCore.Collectables
{
    [Flags]
    public enum Axis
    {
        X = 1,
        Y = 2,
        Z = 4,
    }
    
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Axis _axis;

        [Inject] private readonly GameTime _gameTime;

        private void Start()
        {
            GameContainer.Current.InjectToInstance(this);
        }

        private void Update()
        {
            var rotationVector = Vector3.zero;
            if ((_axis & Axis.X) != 0)
                rotationVector += Vector3.right;
            if ((_axis & Axis.Y) != 0)
                rotationVector += Vector3.up;
            if ((_axis & Axis.Z) != 0)
                rotationVector += Vector3.forward;
            
            float rotationAngle = _speed * _gameTime.DeltaTime;
            transform.rotation *= Quaternion.Euler(rotationVector * rotationAngle);
        }
    }
}