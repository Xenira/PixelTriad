using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Util
{
    public static class DictionarySerializer
    {
        public static object ToObject(this IDictionary<string, object> source, Type t)
        {
            var result = Activator.CreateInstance(t);
            Type resultType = result.GetType();

            foreach (var item in source)
            {
                SetValue(result, resultType, item);
            }

            return result;
        }
        public static T ToObject<T>(this IDictionary<string, object> source)
        {
            return (T)source.ToObject(typeof(T));
        }

        private static void SetValue<T>(T result, Type resultType, KeyValuePair<string, object> item)
        {
            try
            {
                var prop = resultType.GetProperty(item.Key);
                if (prop == null) return;

                var value = item.Value;
                var valueType = item.Value.GetType();
                if (valueType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    value = GetListValue(prop, value, valueType);
                }
                else if (prop.PropertyType.IsDefined(typeof(SerializeAttribute), false))
                {
                    value = ToObject((IDictionary<string, object>)item.Value, prop.PropertyType);
                }
                prop.SetValue(result, Convert.ChangeType(value, prop.PropertyType), null);
            }
            catch (Exception e)
            {
            }
        }

        private static object GetListValue(PropertyInfo prop, object value, Type valueType)
        {
            var listType = prop.PropertyType.GetGenericArguments()[0];
            IList source = (IList)value;
            IList target = (IList)typeof(List<>).MakeGenericType(listType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);

            if (listType.IsDefined(typeof(SerializeAttribute), false))
            {
                foreach (object item in source)
                {
                    target.Add(ToObject((IDictionary<string, object>)item, listType));
                }
                return target;
            }

            return source;
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
