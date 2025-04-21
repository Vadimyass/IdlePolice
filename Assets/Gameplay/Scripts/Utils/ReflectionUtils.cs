using System.Collections.Generic;
using System.Reflection;

namespace Gameplay.Scripts.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Found all fields of type and derrived for object
        /// </summary>
        public static List<T> GetFieldsOfType<T>(object target)
        {
            var result = new List<T>();

            var targetFields = target.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            
            foreach (var fieldInfo in targetFields)
            {
                if (!typeof(T).IsAssignableFrom(fieldInfo.FieldType)) continue;

                result.Add(((T) fieldInfo.GetValue(target)));
            }

            return result;
        }
    }
}