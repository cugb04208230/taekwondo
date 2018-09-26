using System.Text;
using System.Text.Encodings.Web;

namespace Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T To<T>(this object me, T defaultValue = default(T)) where T : struct
        {
            if (me == null)
            {
                return defaultValue;
            }
            try
            {
                return (T)System.Convert.ChangeType(me, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? Nullable<T>(this object me, T? defaultValue = null) where T : struct
        {
            if (me == null)
            {
                return defaultValue;
            }
            try
            {
                return (T)System.Convert.ChangeType(me, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }


	    public static bool IsNotNullOrEmpty(this string value)
	    {
		    return !string.IsNullOrEmpty(value);
	    }

		public static string Formats(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static string UrlEncode(this string value)
        {
            return System.Web.HttpUtility.UrlEncode(value);
        }

        public static string UrlDecode(this string value)
        {
            return System.Web.HttpUtility.UrlDecode(value);
        }
    }
}
