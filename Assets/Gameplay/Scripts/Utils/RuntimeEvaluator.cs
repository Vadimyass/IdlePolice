using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gameplay.Scripts.Utils
{
    public static class RuntimeEvaluator
    {
        private static Dictionary<string, string> s_CachedValues = new Dictionary<string, string>();
        
        public static string EvaluateProperty(string name)
        {
            if (name[0] != '{' || name[name.Length - 1] != '}') return "";

            name = name.Substring(1, name.Length - 2);
            
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cachedValue;
            if (s_CachedValues.TryGetValue(name, out cachedValue))
                return cachedValue;

            int i = name.LastIndexOf('.');
            if (i < 0)
                return name;

            var className = name.Substring(0, i);
            var propName = name.Substring(i + 1);
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = a.GetType(className, false, false);
                if (t == null)
                    continue;
                try
                {
                    var pi = t.GetProperty(propName, BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                    if (pi != null)
                    {
                        var v = pi.GetValue(null, null);
                        if (v != null)
                        {
                            s_CachedValues.Add(name, v.ToString());
                            return v.ToString();
                        }
                    }
                    var fi = t.GetField(propName, BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                    if (fi != null)
                    {
                        var v = fi.GetValue(null);
                        if (v != null)
                        {
                            s_CachedValues.Add(name, v.ToString());
                            return v.ToString();
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return name;
        }
    }
}