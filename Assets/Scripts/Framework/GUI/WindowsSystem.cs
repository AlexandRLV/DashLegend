using System;
using System.Collections.Generic;
using Framework.DI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.GUI
{
    public class WindowsSystem
    {
        private readonly UiRoot _uiRoot;
        
        private readonly Dictionary<Type, WindowBase> _windowsPrefabs;
        private readonly Dictionary<Type, WindowBase> _loadedWindows;
        private readonly Stack<WindowBase> _windowsStack;
        
        [Inject]
        public WindowsSystem(GameWindows gameWindows, UiRoot uiRoot)
        {
            _uiRoot = uiRoot;
            _windowsPrefabs = new Dictionary<Type, WindowBase>();
            _loadedWindows = new Dictionary<Type, WindowBase>();
            _windowsStack = new Stack<WindowBase>();

            foreach (var window in gameWindows.Windows)
            {
                _windowsPrefabs.Add(window.GetType(), window);
            }
        }

        public void PushWindow<T>() where T : WindowBase
        {
            var window = CreateWindow<T>();
            if (_windowsStack.TryPeek(out var currentWindow))
                currentWindow.gameObject.SetActive(false);
            
            _windowsStack.Push(window);
        }

        public void PushWindow<TWindow, TInitData>(TInitData initData)
            where TWindow : WindowBase, IInitializableWindow<TInitData>
            where TInitData : struct
        {
            var window = CreateWindow<TWindow, TInitData>(initData);
            CleanupStack();
            
            if (_windowsStack.TryPeek(out var currentWindow))
                currentWindow.gameObject.SetActive(false);
            
            _windowsStack.Push(window);
        }

        public void PopWindow(WindowBase window)
        {
            DestroyWindow(window);
            CleanupStack(window);
            
            if (_windowsStack.TryPeek(out var currentWindow))
                currentWindow.gameObject.SetActive(true);
        }

        public void PopWindow<T>() where T : WindowBase
        {
            if (!TryGetWindow(out T window))
                return;
            
            DestroyWindow(window);
            CleanupStack(window);
            
            if (_windowsStack.TryPeek(out var currentWindow))
                currentWindow.gameObject.SetActive(true);
        }

        public bool TryGetWindow<T>(out T window) where T : WindowBase
        {
            window = null;
            var type = typeof(T);
            if (_loadedWindows.TryGetValue(type, out var baseWindow))
            {
                if (baseWindow is not T targetWindow)
                    throw new ArgumentException($"Error in getting window type {type.Name} - have cached wrong type of window");

                window = targetWindow;
                return true;
            }

            return false;
        }

        public T CreateWindow<T>() where T : WindowBase
        {
            var type = typeof(T);
            if (_loadedWindows.TryGetValue(type, out var baseWindow))
            {
                if (baseWindow is not T targetWindow)
                    throw new ArgumentException($"Error in creating window type {type.Name} - already created wrong type of window");

                return targetWindow;
            }
            
            if (!_windowsPrefabs.TryGetValue(type, out var windowPrefabBase))
                throw new ArgumentException($"Error in getting window type {type.Name} - window not registered");
            
            if (windowPrefabBase is not T windowPrefab)
                throw new ArgumentException($"Error in getting window type {type.Name} - registered wrong window");
            
            var window = GameContainer.Current.InstantiateAndResolve(windowPrefab, _uiRoot.WindowsParent);
            _loadedWindows.Add(type, window);
            return window;
        }

        public TWindow CreateWindow<TWindow, TInitData>(TInitData initData)
            where TWindow : WindowBase, IInitializableWindow<TInitData>
            where TInitData : struct
        {
            var type = typeof(TWindow);
            if (_loadedWindows.TryGetValue(type, out var baseWindow))
            {
                if (baseWindow is not TWindow targetWindow)
                    throw new ArgumentException($"Error in creating window type {type.Name} - already created wrong type of window");

                return targetWindow;
            }
            
            if (!_windowsPrefabs.TryGetValue(type, out var windowPrefabBase))
                throw new ArgumentException($"Error in getting window type {type.Name} - window not registered");
            
            if (windowPrefabBase is not TWindow windowPrefab)
                throw new ArgumentException($"Error in getting window type {type.Name} - registered wrong window");
            
            var window = GameContainer.Current.InstantiateAndResolve(windowPrefab, _uiRoot.WindowsParent);
            _loadedWindows.Add(type, window);
            window.Initialize(initData);
            return window;
        }

        private void CleanupStack(WindowBase deletingWindow = null)
        {
            while (_windowsStack.TryPeek(out var stackedWindow) && (stackedWindow == null || (deletingWindow != null && stackedWindow == deletingWindow)))
                _windowsStack.Pop();
        }

        private void DestroyWindow(WindowBase window)
        {
            if (window == null)
            {
                Debug.LogError("[WindowsSystem] Trying to destroy null window");
                return;
            }
            
            var type = window.GetType();
            
            if (!_loadedWindows.TryGetValue(type, out var loadedWindow))
            {
                if (window != null)
                {
                    window.Destroy();
                    Object.Destroy(window.gameObject);
                }
                
                return;
            }

            if (window != null && loadedWindow != window)
                throw new ArgumentException($"Trying to destroy {type.Name} window, but saved different object!");

            CleanupStack(loadedWindow);
            if (loadedWindow != null && loadedWindow.gameObject != null)
            {
                loadedWindow.Destroy();
                Object.Destroy(loadedWindow.gameObject);
            }
            
            _loadedWindows.Remove(type);
        }

        public void DestroyAll()
        {
            foreach (var loadedWindow in _loadedWindows)
            {
                if (loadedWindow.Value != null && loadedWindow.Value.gameObject != null)
                    Object.Destroy(loadedWindow.Value.gameObject);
            }
            
            _loadedWindows.Clear();
        }
    }
}