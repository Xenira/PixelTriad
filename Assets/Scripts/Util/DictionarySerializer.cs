using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public static class DictionarySerializer
    {
        public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                var prop = someObjectType.GetProperty(item.Key);
                if (prop.GetType().IsSerializable)
                {
                    prop.SetValue(someObject, ToObject((IDictionary<string, object>)item.Value, prop.GetType()), null);
                }
                else
                {
                    prop.SetValue(someObject, item.Value, null);
                }
            }

            return someObject;
        }

        public static T ToObject<T>(this IDictionary<string, object> source, T entity) where T : class
        {
            T someObject = (T)Activator.CreateInstance(typeof(T));
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                var prop = someObjectType.GetProperty(item.Key);
                if (prop.GetType().IsSerializable)
                {
                    prop.SetValue(someObject, ToObject((IDictionary<string, object>)item.Value, prop.GetType()), null);
                }
                else
                {
                    prop.SetValue(someObject, item.Value, null);
                }
            }

            return someObject;
        }
    }
}
