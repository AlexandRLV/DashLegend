using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Framework
{
    public readonly struct ReadOnlyList<T> : IEquatable<ReadOnlyList<T>>
    {
        private readonly List<T> _list;
        
        public ReadOnlyList(List<T> list)
        {
            _list = list;
        }
        
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list[index];
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list?.Count ?? 0;
        }
        
        public static implicit operator ReadOnlyList<T>(List<T> list) => new ReadOnlyList<T>(list);
        public static bool operator ==(in ReadOnlyList<T> left, object right) => left._list == right;
        public static bool operator ==(object left, in ReadOnlyList<T> right) => left == right._list;
        public static bool operator !=(in ReadOnlyList<T> left, object right) => left._list != right;
        public static bool operator !=(object left, in ReadOnlyList<T> right) => left != right._list;
        public override bool Equals(object obj) => obj != null && obj == _list;
        public bool Equals(ReadOnlyList<T> other) => other._list == _list;
        public override int GetHashCode() => (_list != null ? _list.GetHashCode() : 0);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_list);

        public ref struct Enumerator
        {
            private readonly List<T> _list;
            private int _index;
        
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(List<T> list)
            {
                _list = list;
                _index = -1;
            }
        
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int index = _index + 1;
                if (index < _list.Count)
                {
                    _index = index;
                    return true;
                }

                return false;
            }
        
            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _list[_index];
            }
        }
    }
}