using System;
using UnityEngine;
using VContainer;

namespace Framework.GUI
{
    public abstract class WindowBase : MonoBehaviour, IDisposable
    {
        [Inject] private WindowsSystem _windowsSystem;

        public virtual void Destroy() { }
        
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