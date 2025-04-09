using System;
using System.Text;
using Framework.Pools;

namespace Framework.Scoped
{
    public class StringBuilderContainer : IDisposable
    {
        public StringBuilder Value { get; } = new(); // Containers are pooled, so this value will be reused

        public void Dispose()
        {
            Value.Clear();
        }
    }
    
    public struct ScopedStringBuilder : IDisposable
    {
        private StringBuilderContainer _value;
        
        public void Dispose()
        {
            PlainSharpObjectsPool<StringBuilderContainer>.shared.Return(_value);
        }

        public static ScopedStringBuilder Get(out StringBuilder stringBuilder)
        {
            var scopedContainer = new ScopedStringBuilder
            {
                _value = PlainSharpObjectsPool<StringBuilderContainer>.shared.Get()
            };

            stringBuilder = scopedContainer._value.Value;
            stringBuilder.Clear();
            return scopedContainer;
        }
    }
}