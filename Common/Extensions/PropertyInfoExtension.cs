using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class PropertyUtil
    {

        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetJsonProperty(this PropertyInfo value)
        {
            // 获取描述的属性。
            JsonPropertyAttribute attr = Attribute.GetCustomAttribute(value,
                typeof(JsonPropertyAttribute), false) as JsonPropertyAttribute;
            return attr?.PropertyName;
        }
    }
}
