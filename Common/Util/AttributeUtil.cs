using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Common.Util
{
    public static class AttributeUtil
    {

        public static T GetCustomAttribute<T>(this FieldInfo source) where T : Attribute
        {
            object[] attributes = source.GetCustomAttributes(typeof(T), false);
            return attributes.OfType<T>().FirstOrDefault();
        }


	    public static int GetValueByDescription<T>(this string description)
	    {
		    var type = typeof(T);
		    if (type.IsEnum)
		    {
			    foreach (var value in Enum.GetValues(type))
			    {
				    var item = (Enum)Enum.Parse(type, value.ToString());
				    var desc = item.GetDescription();
				    if (description == desc)
				    {
					    return (int)Enum.Parse(type, value.ToString());
				    }
			    }
		    }
		    return -1;
	    }

		/// <summary>
		/// 返回枚举项的描述信息。
		/// </summary>
		/// <param name="value">要获取描述信息的枚举项。</param>
		/// <returns>枚举想的描述信息。</returns>
		public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 返回对象的描述信息。
        /// </summary>
        /// <param name="type">要获取描述信息项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetDescription(this Type type)
        {
            // 获取描述的属性。
            DescriptionAttribute attr = Attribute.GetCustomAttribute(type,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }
            return string.Empty;
        }


	    /// <summary>
	    /// 返回对象的描述信息。
	    /// </summary>
	    /// <param name="type">要获取描述信息项。</param>
	    /// <returns>枚举想的描述信息。</returns>
	    public static bool HasNotMapped(this PropertyInfo type)
	    {
			// 获取描述的属性。
		    NotMappedAttribute attr = Attribute.GetCustomAttribute(type,
			    typeof(NotMappedAttribute), false) as NotMappedAttribute;
		    if (attr != null)
		    {
			    return true;
		    }
		    return false;
	    }

		/// <summary>
		/// 返回属性的描述信息。
		/// </summary>
		/// <param name="type">要获取描述信息项。</param>
		/// <returns>枚举想的描述信息。</returns>
		public static string GetDescription(this PropertyInfo type)
        {
            // 获取描述的属性。
            DescriptionAttribute attr = Attribute.GetCustomAttribute(type,
                typeof(DescriptionAttribute), false) as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }
            return string.Empty;
        }
    }
}
