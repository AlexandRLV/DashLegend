using System.Text;
using UnityEngine;

namespace Framework.Extensions
{
    public static class Extensions
    {
        private static StringBuilder _stringBuilder;
        
        public static void ToLocalZero(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public static Quaternion FlatRotation(this Transform transform)
        {
            float eulerY = transform.eulerAngles.y;
            return Quaternion.Euler(0f, eulerY, 0f);
        }
        
        public static void MoveToLocalZero(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);

        public static string ToShortString(this long value)
        {
            _stringBuilder ??= new StringBuilder();
            _stringBuilder.Clear();

            if (value < 1000) return value.ToString();

            value /= 1000;
            if (value < 1000)
            {
                _stringBuilder.Append(value);
                _stringBuilder.Append("K"); // TODO: localization
                return _stringBuilder.ToString();
            }

            value /= 1000;
            if (value < 1000)
            {
                _stringBuilder.Append(value);
                _stringBuilder.Append("M"); // TODO: localization
                return _stringBuilder.ToString();
            }

            value /= 1000;
            if (value < 1000)
            {
                _stringBuilder.Append(value);
                _stringBuilder.Append("B"); // TODO: localization
                return _stringBuilder.ToString();
            }

            return "999B+";
        }

        public static Color WithR(this Color value, float r)
        {
            value.r = r;
            return value;
        }

        public static Color WithA(this Color value, float a)
        {
            value.a = a;
            return value;
        }
    }
}