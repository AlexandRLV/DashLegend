using UnityEngine;

namespace Framework.Extensions
{
    public static class MathExtensions
    {
        public static bool InRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static void IncreaseInRange(this ref int value, int max, bool include = false)
        {
            value++;
            if (value > max || value == max && !include)
                value = 0;
        }

        public static void DecreaseInRange(this ref int value, int max, bool include = false)
        {
            value--;
            if (value < 0)
                value = include ? max : max - 1;
        }

        public static Vector3 WithY(this Vector3 origin, float y)
        {
            origin.y = y;
            return origin;
        }

        public static Vector3 FlatVector(this Vector3 value)
        {
            value.y = 0f;
            return value;
        }

        public static void SetYPosition(this Transform transform, float value)
        {
            var position = transform.position;
            position.y = value;
            transform.position = position;
        }
		
        public static bool IsNaNOrInfinity(this Vector3 v)
        {
            return IsNaN(v) || IsInfinity(v);
        }
		
        public static bool IsNaN(this Vector3 v)
        {
            return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
        }
        
        public static bool IsInfinity(this Vector3 v)
        {
            return float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z);
        }
        
        public static bool IsNaN(this Quaternion v)
        {
            return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z) || float.IsNaN(v.w);
        }
        
        public static bool IsInfinity(this Quaternion v)
        {
            return float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z) || float.IsInfinity(v.w);
        }
    }
}