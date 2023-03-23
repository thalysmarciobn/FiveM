﻿using System.Collections.Generic;
using System.Dynamic;

namespace Client.Extensions
{
    public static class CoreExtensions
    {
        public static ExpandoObject GetObject(this IDictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out var value))
                return value as ExpandoObject;
            return null;
        }

        public static string GetString(this IDictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out var value))
                return value.ToString();
            return null;
        }

        public static int GetInt(this IDictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out var value))
                return int.TryParse(value.ToString(), out var result) ? result : 0;

            return -1;
        }

        public static bool GetBool(this IDictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out var value))
                return bool.TryParse(value.ToString(), out var result) && result;

            return false;
        }

        public static float GetFloat(this IDictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out var value))
                return float.TryParse(value.ToString(), out var result) ? result : 0;

            return -1;
        }
    }
}