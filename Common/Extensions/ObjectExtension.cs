using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class ObjectExtension
    {

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var type = obj.GetType();
            foreach (var fieldInfo in type.GetProperties())
            {
                var value = fieldInfo.GetValue(obj);
                result.Add(fieldInfo.Name,value);
            }
            return result;
        }

        public static string SerializeObject(this object obj)
        {
            var result = string.Empty;
            if (obj != null)
            {
                result=JsonConvert.SerializeObject(obj);
            }
            return result;
        }

        public static T ConvertTo<T>(this object obj)
        {
            Type from = obj.GetType();
            Type to = typeof(T);
            PropertyInfo[] fromProperties = from.GetProperties();
            PropertyInfo[] toProperties = to.GetProperties();

            object result = to.Assembly.CreateInstance(to.FullName);
            foreach (var property in toProperties)
            {
                var targetProperties = fromProperties.Where
                    (p => p.Name == property.Name && p.PropertyType == property.PropertyType).ToArray();
                if (targetProperties.Any())
                {
                    property.SetValue(result, targetProperties.First().GetValue(obj));
                }
            }

            return (T)result;
        }
    }
}
