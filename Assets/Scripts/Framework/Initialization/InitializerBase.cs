using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Framework.Initialization
{
    public abstract class InitializerBase : MonoBehaviour
    {
        public abstract UniTask Initialize();
        public abstract void Dispose();
    }
}