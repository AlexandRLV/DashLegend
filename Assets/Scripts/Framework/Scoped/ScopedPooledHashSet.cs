using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Framework.Utils
{
    public struct ScopedPooledHashSet<T> : IDisposable
    {
        public HashSet<T> value;
        
        public void Dispose()
        {
            HashSetPool<T>.Release(value);
        }

        public static ScopedPooledHashSet<T> Create()
        {
            return new ScopedPooledHashSet<T>
            {
                value = HashSetPool<T>.Get()
            };
        }
    }
}