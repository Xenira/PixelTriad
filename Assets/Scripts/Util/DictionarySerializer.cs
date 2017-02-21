using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Util
{
    public static class DictionarySerializer
    {
        public static T ToObject<T>(this IDictionary<string, object> source) where T : class
        {
            return source.ToObject(typeof(T) as T);
        }
        public static T ToObject<T>(this IDictionary<string, object> source, T type) where T : class
        {
            T someObject = (T)Activator.CreateInstance(typeof(T));
            Type someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                var prop = someObjectType.GetProperty(item.Key);
                if (prop == null) continue;
                if (prop.PropertyType.IsDefined(typeof(SerializeAttribute), false))
                {
                    prop.SetValue(someObject, ToObject((IDictionary<string, object>)item.Value, prop.GetType()), null);
                }
                else
                {
                    prop.SetValue(someObject, Convert.ChangeType(item.Value, prop.PropertyType), null);
                }
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr)
                .Select(prop =>
                {
                    object value = prop.GetValue(source, null);
                    if (prop.PropertyType.IsDefined(typeof(SerializeAttribute), false))
                    {
                        value = AsDictionary(value);
                    }
                    return new KeyValuePair<string, object>(prop.Name, value);
                }).ToDictionary(e => e.Key, e => e.Value);
        }
    }
}
