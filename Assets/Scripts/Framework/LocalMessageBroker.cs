using System;
using System.Collections.Generic;

namespace Framework
{
    public delegate void ActionRef<T>(in T message) where T : struct;

    public interface IMessageListener<T> where T : struct
    {
        public void OnMessage(in T message);
    }
    
    public class LocalMessageBroker
    {
        private interface IListenersContainer { }
        
        private class ActionListenersContainer<T> : IListenersContainer where T : struct
        {
            public readonly List<ActionRef<T>> Listeners = new();
        }
        
        private class InterfaceListenersContainer<T> : IListenersContainer where T : struct
        {
            public readonly List<IMessageListener<T>> Listeners = new();
        }

        private readonly Dictionary<Type, IListenersContainer> _actionHandlers = new();
        private readonly Dictionary<Type, IListenersContainer> _interfaceHandlers = new();

        public void Subscribe<T>(ActionRef<T> callback) where T : struct
        {
            var type = typeof(T);
            ActionListenersContainer<T> container;
            if (_actionHandlers.TryGetValue(type, out var iContainer))
            {
                container = iContainer as ActionListenersContainer<T>;
                if (container == null)
                    throw new ArgumentException($"Subscribe to local event error: wrong receiver type: {type.FullName}");
            }
            else
            {
                container = new ActionListenersContainer<T>();
                _actionHandlers.Add(type, container);
            }
            
            container.Listeners.Add(callback);
        }

        public void Unsubscribe<T>(ActionRef<T> callback) where T : struct
        {
            var type = typeof(T);
            if (!_actionHandlers.TryGetValue(type, out var iContainer))
                return;
            
            if (iContainer is not ActionListenersContainer<T> container)
                throw new ArgumentException($"Unsubscribe from local event error: wrong receiver type: {type.FullName}");

            container.Listeners.Remove(callback);
        }
        
        public void Subscribe<T>(IMessageListener<T> listener) where T : struct
        {
            var type = typeof(T);
            InterfaceListenersContainer<T> container;
            if (_interfaceHandlers.TryGetValue(type, out var iContainer))
            {
                container = iContainer as InterfaceListenersContainer<T>;
                if (container == null)
                    throw new ArgumentException($"Subscribe to local event error: wrong receiver type: {type.FullName}");
            }
            else
            {
                container = new InterfaceListenersContainer<T>();
                _interfaceHandlers.Add(type, container);
            }
            
            container.Listeners.Add(listener);
        }

        public void Unsubscribe<T>(IMessageListener<T> listener) where T : struct
        {
            var type = typeof(T);
            if (!_interfaceHandlers.TryGetValue(type, out var iContainer))
                return;
            
            if (iContainer is not InterfaceListenersContainer<T> container)
                throw new ArgumentException($"Unsubscribe from local event error: wrong receiver type: {type.FullName}");

            container.Listeners.Remove(listener);
        }

        public void TriggerEmpty<T>() where T : struct
        {
            var message = new T();
            Trigger(message);
        }

        public void Trigger<T>(in T message) where T : struct
        {
            var type = typeof(T);
            if (_actionHandlers.TryGetValue(type, out var iContainer))
            {
                if (iContainer is not ActionListenersContainer<T> container)
                    throw new ArgumentException($"Trigger local event error: wrong action receiver type: {type.FullName}");

                foreach (var listener in container.Listeners)
                {
                    listener.Invoke(message);
                }
            }

            if (_interfaceHandlers.TryGetValue(type, out iContainer))
            {
                if (iContainer is not InterfaceListenersContainer<T> container)
                    throw new ArgumentException($"Trigger local event error: wrong interface receiver type: {type.FullName}");
                
                foreach (var listener in container.Listeners)
                {
                    listener.OnMessage(message);
                }
            }
        }
    }
}