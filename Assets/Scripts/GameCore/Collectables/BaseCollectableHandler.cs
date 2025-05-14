using System;
using Framework.DI;

namespace GameCore.Collectables
{
    public abstract class BaseCollectableHandler
    {
        public abstract void HandleCollectable(BaseCollectable collectable);
    }

    public abstract class BaseCollectableHandler<T> : BaseCollectableHandler, IInitializable, IDisposable where T : BaseCollectable
    {
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;

        public void Initialize()
        {
            _collectablesSpawner.RegisterHandler(typeof(T), this);
        }

        public void Dispose()
        {
            _collectablesSpawner.UnregisterHandler(typeof(T));
        }
        
        public override void HandleCollectable(BaseCollectable collectable)
        {
            if (collectable is T typedCollectable)
                HandleCollectable(typedCollectable);
        }

        protected abstract void HandleCollectable(T collectable);
    }
}