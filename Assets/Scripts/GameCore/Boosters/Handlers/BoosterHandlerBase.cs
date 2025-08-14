using System;
using VContainer;
using VContainer.Unity;

namespace GameCore.Boosters.Handlers
{
    public abstract class BoosterHandlerBase : IInitializable, IDisposable
    {
        protected abstract BoosterType Type { get; }

        [Inject] private readonly BoostersService _boostersService;
        
        public void Initialize()
        {
            _boostersService.RegisterBoosterHandler(Type, this);
            InitializeInternal();
        }

        public void Dispose()
        {
            _boostersService.UnregisterHandler(Type, this);
        }
        
        public abstract void Enable();
        public abstract void Update();
        public abstract void Disable();

        protected abstract void InitializeInternal();
    }
}