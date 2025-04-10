using System;
using Framework.Pools;

namespace Framework.Initialization
{
    public struct InitializeOperationContainer : IDisposable
    {
        public bool IsDone => Operation.IsDone;
        public float Progress => Operation.Progress;
        
        public InitializeOperation Operation;
        
        public void Dispose()
        {
            PlainSharpObjectsPool<InitializeOperation>.Shared.Return(Operation);
        }

        public static InitializeOperationContainer Create()
        {
            return new InitializeOperationContainer
            {
                Operation = PlainSharpObjectsPool<InitializeOperation>.Shared.Get()
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