using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class SerializableContainer<T1, T2>
    {
        [SerializeField] public T1 Item1;
        [SerializeField] public T2 Item2;
    }
    
    [Serializable]
    public class SerializableContainer<T1, T2, T3>
    {
        [SerializeField] public T1 Item1;
        [SerializeField] public T2 Item2;
        [SerializeField] public T3 Item3;
    }
}