using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Framework.Scoped
{
    public struct ScopedPooledList<T> : IDisposable
    {
        public List<T> value;
        
        public void Dispose()
        {
            ListPool<T>.Release(value);
        }

        public static ScopedPooledList<T> Create()
        {
            return new ScopedPooledList<T>
            {
                value = ListPool<T>.Get()
            };
        }
    }
}