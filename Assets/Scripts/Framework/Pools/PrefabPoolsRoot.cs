using UnityEngine;

namespace Framework.Pools
{
    public static class PrefabPoolsRoot
    {
        public static GameObject Root { get; private set; }
        
        public static void CreateMainRoot()
        {
            Root = new GameObject("Prefab Pools");
        }
    }
}