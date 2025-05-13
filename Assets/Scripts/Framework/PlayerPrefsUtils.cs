using UnityEngine;

namespace Framework
{
    public static class PlayerPrefsUtils
    {
        public static float GetKeyOrDefault(string key, float defaultValue)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
        }
        
        public static int GetKeyOrDefault(string key, int defaultValue)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
        }
        
        public static string GetKeyOrDefault(string key, string defaultValue)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
        }
    }
}