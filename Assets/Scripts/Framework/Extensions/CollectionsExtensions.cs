using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

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
        
        public static T GetRandomWithChance<T>(this T[] array) where T : ICollectionWithChanceItem
        {
            if (array == null || array.Length == 0)
                return default;

            float totalChance = 0f;
            foreach (var item in array)
            {
                totalChance += item.Chance;
            }

            float chanceValue = Random.Range(0f, totalChance);
            foreach (var item in array)
            {
                if (item.Chance > chanceValue)
                    return item;

                chanceValue -= item.Chance;
            }

            return default;
        }

        public static T GetRandomWithChance<T>(this List<T> list) where T : ICollectionWithChanceItem
        {
            if (list == null || list.Count == 0)
                return default;
            
            float totalChance = 0f;
            foreach (var item in list)
            {
                totalChance += item.Chance;
            }

            float chanceValue = Random.Range(0f, totalChance);
            foreach (var item in list)
            {
                if (item.Chance > chanceValue)
                    return item;

                chanceValue -= item.Chance;
            }

            return default;
        }
        
        public static bool TryGetValueByKey<T1, T2, T3>(this T3[] array, T1 key, out T2 value)
            where T1 : IEquatable<T1>
            where T3 : SerializableContainer<T1, T2>
        {
            value = default;
            if (array == null || array.Length == 0)
                return false;

            foreach (var item in array)
            {
                if (item.Item1.Equals(key))
                {
                    value = item.Item2;
                    return true;
                }
            }

            return false;
        }
        
        public static bool TryGetValueByEnumKey<T1, T2, T3>(this T3[] array, T1 key, out T2 value)
            where T1 : Enum
            where T3 : SerializableContainer<T1, T2>
        {
            value = default;
            if (array == null || array.Length == 0)
                return false;

            foreach (var item in array)
            {
                if (item.Item1.Equals(key))
                {
                    value = item.Item2;
                    return true;
                }
            }

            return false;
        }
    }
}