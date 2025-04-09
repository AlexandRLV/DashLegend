using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Framework.Utils
{
    public struct ScopedPooledDictionary<TKey, TValue> : IDisposable
    {
        public Dictionary<TKey, TValue> value;
        
        public void Dispose()
        {
            DictionaryPool<TKey, TValue>.Release(value);
        }

        public static ScopedPooledDictionary<TKey, TValue> Create()
        {
            return new ScopedPooledDictionary<TKey, TValue>
            {
                value = DictionaryPool<TKey, TValue>.Get()
            };
        }
    }
}