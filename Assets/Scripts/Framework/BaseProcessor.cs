using UnityEngine;

namespace Framework
{
    public abstract class BaseProcessor<T> where T : struct
    {
        public bool Process(ref T data, bool noValidate = false)
        {
            Debug.Log($"Process {typeof(T).Name}");
            if (!noValidate && !Validate(ref data))
                return false;
            
            return ProcessInternal(ref data);
        }

        protected virtual bool Validate(ref T data) => true;

        protected abstract bool ProcessInternal(ref T data);
    }
}