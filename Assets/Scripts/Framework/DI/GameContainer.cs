using System.Collections.Generic;

namespace Framework.DI
{
    public static class GameContainer
    {
        public static Container Current { get; private set; }

        private static readonly Stack<Container> _containers = new();
        
        public static Container CreateNew()
        {
            var container = new Container(Current);
            _containers.Push(container);
            Current = container;
            return container;
        }

        public static void DisposeContainer(Container container)
        {
            while (_containers.TryPeek(out var containerInStack))
            {
                if (!HasParentInHierarchy(containerInStack, container))
                    break;
                
                containerInStack.Dispose();
                _containers.Pop();
            }
            
            Current = _containers.Peek();
        }

        private static bool HasParentInHierarchy(Container child, Container parent)
        {
            if (child == parent)
                return true;
            
            while (child.Parent != null)
            {
                if (child.Parent == parent)
                    return true;

                child = child.Parent;
            }

            return false;
        }
    }
}