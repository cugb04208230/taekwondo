using Common.Di;
using Common.Extensions;

namespace Common
{
    public static class Settings
    {
        public static string GetConfiguration(string key)
        {
            return ObjectContainer.Instance.AppConfiguration.GetSection(key).Value;
        }

        public static string GetBasePath(string area)
        {
            return GetConfiguration($"Settings:BasePath:{area}");
        }

        public static T GetSetting<T>(string key) where T : struct 
        {
            return GetConfiguration($"Settings:{key}").To<T>(new T());
        }


        public static string GetSetting(string key)
        {
            return GetConfiguration($"Settings:{key}");
        }

        public static string ApiHost => GetSetting("ApiHost");

        public static string PicFtp => GetSetting("PicFtp");

        #region Auth

        public static string AlipayLoginAppId=> GetSetting("AlipayLoginAppId");
        public static string AlipayGetway => GetSetting("AlipayGetway");
        public static string AlipayLoginPrivateKey => GetSetting("AlipayLoginPrivateKey");
        public static string AlipayLoginPublickKey => GetSetting("AlipayLoginPublickKey");
        public static string AlipayAuthCallBack => GetSetting("AlipayAuthCallBack");
        public static string AlipayLoginAlipayPublickKey => GetSetting("AlipayLoginAlipayPublickKey");

        public static string WeChatLoginAppId=> GetSetting("WeChatLoginAppId");
        public static string WeChatLoginAppSecret => GetSetting("WeChatLoginAppSecret");
        public static string WeChatLoginCallBackUrl => GetSetting("WeChatLoginCallBackUrl");
        

        #endregion

        public static string ConnectionString => GetConfiguration("ConnectionStrings:MsSqlContext");

    }
}
