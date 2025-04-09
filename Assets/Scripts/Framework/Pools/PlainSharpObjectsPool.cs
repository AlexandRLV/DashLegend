using System;
using System.Collections.Generic;

namespace Framework.Pools
{
    public class PlainSharpObjectsPool<T> where T : IDisposable, new()
    {
        public static PlainSharpObjectsPool<T> shared = new();
		
        private readonly Queue<T> _queue;
		
        private PlainSharpObjectsPool() => _queue = new Queue<T>();

        public T Get() => _queue.TryDequeue(out var result) ? result : new T();

        public void Return(T value)
        {
            value.Dispose();
            _queue.Enqueue(value);
        }
    }
}