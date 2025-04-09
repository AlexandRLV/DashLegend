using System.Collections.Generic;
using UnityEngine;

namespace Framework.Extensions
{
    public static class CollectionsExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return default;

            return array[Random.Range(0, array.Length)];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[Random.Range(0, list.Count)];
        }
    }
}