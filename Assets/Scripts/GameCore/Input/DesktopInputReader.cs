using System;
using Framework.DI;
using Framework.MonoUpdate;
using UnityEngine;

namespace GameCore.Input
{
    public class DesktopInputReader : IDisposable, IUpdatable, IInputSource
    {
        [Inject] private readonly MonoUpdater _monoUpdater;

        public bool JumpPressed { get; private set; }

        public void Initialize()
        {
            _monoUpdater.AddUpdatable(this);
        }

        public void Dispose()
        {
            _monoUpdater.RemoveUpdatable(this);
        }

        public void Update()
        {
            JumpPressed = UnityEngine.Input.GetKeyDown(KeyCode.Space);
        }
    }
}