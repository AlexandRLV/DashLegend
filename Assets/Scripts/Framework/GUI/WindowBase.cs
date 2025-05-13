using System;
using Framework.DI;
using UnityEngine;

namespace Framework.GUI
{
    public abstract class WindowBase : MonoBehaviour, IDisposable
    {
        [Inject] private WindowsSystem _windowsSystem;

        protected virtual void Start()
        {
            GameContainer.Current.AddDisposable(this);
        }

        public virtual void Destroy()
        {
            GameContainer.Current.RemoveDisposable(this);
        }

        protected void Close()
        {
            _windowsSystem.PopWindow(this);
        }

        protected void PushWindow<T>() where T : WindowBase
        {
            _windowsSystem.PushWindow<T>();
        }

        protected T CreateWindow<T>() where T : WindowBase
        {
            return _windowsSystem.CreateWindow<T>();
        }

        public void Dispose()
        {
            Close();
        }
    }
}