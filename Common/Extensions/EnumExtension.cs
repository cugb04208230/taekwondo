using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Common.Util;

namespace Common.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举遍历
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> ToKeyValuePairs(this Type type)
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            if (type.IsEnum)
            {

                var values = Enum.GetValues(type);
                for (int i = 0; i < values.Length; i++)
                {
                    var en = Enum.Parse(type, values.GetValue(i).ToString());
                    // 获取枚举字段。
                    FieldInfo fieldInfo = type.GetField(values.GetValue(i).ToString());
                    if (fieldInfo != null)
                    {
                        // 获取描述的属性。
                        DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                            typeof(DescriptionAttribute), false) as DescriptionAttribute;
                        if (attr != null)
                        {
                            result.Add(new KeyValuePair<string, int>(attr.Description, (int)en));
                        }
                    }
                }

            }
            return result;
        }
        public static Dictionary<string,string> ToDictionary(this Type type)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (type.IsEnum)
            {

                var values = Enum.GetValues(type);
                for (int i = 0; i < values.Length; i++)
                {
                    var en = Enum.Parse(type, values.GetValue(i).ToString());
                    // 获取枚举字段。
                    FieldInfo fieldInfo = type.GetField(values.GetValue(i).ToString());
                    if (fieldInfo != null)
                    {
                        // 获取描述的属性。
                        DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                            typeof(DescriptionAttribute), false) as DescriptionAttribute;
                        if (attr != null)
                        {
                            result.Add(((int)en).ToString(), attr.Description);
                        }
                    }
                }

            }
            return result;
        }

	    public static List<int> ToValueList(this Type type)
	    {
		    List<int> result = new List<int>();
		    if (type.IsEnum)
		    {

			    var values = Enum.GetValues(type);
			    for (int i = 0; i < values.Length; i++)
			    {
				    var en = Enum.Parse(type, values.GetValue(i).ToString());
				    result.Add((int)en);
			    }

		    }
		    return result;
	    }
	}

    public class EnumUtil
    {
        public static Dictionary<string, string> ToDictionary(Type enumType)
        {
            var dictionary = new Dictionary<string, string>();
            if (enumType.IsEnum)
            {
                foreach (var value in Enum.GetValues(enumType))
                {
                    var item = (Enum)Enum.Parse(enumType, value.ToString());
                    var desc = item.GetDescription();
                    dictionary[value.ToString()] = desc;
                }
            }
            return dictionary;
        }
    }
}
