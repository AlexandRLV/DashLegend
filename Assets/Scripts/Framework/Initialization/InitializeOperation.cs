using System;
using Framework.Pools;

namespace Framework.Initialization
{
    public struct InitializeOperationContainer : IDisposable
    {
        public bool IsDone => Value.IsDone;
        public float Progress => Value.Progress;
        
        public InitializeOperation Value;
        
        public void Dispose()
        {
            PlainSharpObjectsPool<InitializeOperation>.Shared.Return(Value);
        }

        public static InitializeOperationContainer Create()
        {
            return new InitializeOperationContainer
            {
                Value = PlainSharpObjectsPool<InitializeOperation>.Shared.Get()
            };
        }
    }

    public class InitializeOperation : IDisposable
    {
        public bool IsDone;
        public float Progress;
        
        public void Dispose()
        {
            IsDone = false;
            Progress = 0f;
        }
    }
}