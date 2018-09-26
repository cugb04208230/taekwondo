using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Common.Extensions;
using Common.Log;
using Newtonsoft.Json;

namespace Common.Authorized.Login
{
    public static class WeChatLogin
    {

        private static string AppId => Settings.WeChatLoginAppId;
        private static string RedirectUrl => Settings.WeChatLoginCallBackUrl;
        private static string Secret => Settings.WeChatLoginAppSecret;
        /// <summary>
        /// 获取会话的UUID
        /// </summary>
        public static string UrlGetUuid = $"https://open.weixin.qq.com/connect/qrconnect?appid={AppId}&redirect_uri={RedirectUrl}&response_type=code&scope=snsapi_login&state=";

        public static string UrlQrCode = "https://open.weixin.qq.com/connect/qrcode/";
        /// <summary>
        /// 获取二维码的URL
        /// </summary>
        public static string UrlAuthCode = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={AppId}&secret={Secret}&grant_type=authorization_code&code=";
        /// <summary>
        /// 等待扫码登陆
        /// </summary>
        public static string UrlWaitLogin = "https://long.open.weixin.qq.com/connect/l/qrconnect?uuid={0}&_={1}";
        /// <summary>
        /// 更新身份票据
        /// </summary>
        public static string UrlRefreshToken =
            $"https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={AppId}&grant_type=refresh_token&refresh_token={0}";
        /// <summary>
        /// 获取用户信息
        /// </summary>
        public static string UrlUserInfo =
            "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}";


        /// <summary>
        /// 获取登录的二维码
        /// </summary>
        /// <returns></returns>
        public static string GetQrCode(long id)
        {
            var uuid = GetUuid(id);
            return UrlQrCode+uuid;
        }
        private static int TimeStamp()
        {
            return (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        #region Http
        /// <summary>
        /// 向服务器发送Request
        /// </summary>
        /// <param name="url">字符串</param>
        /// <param name="method">枚举类型的方法Get或者Post</param>
        /// <param name="body">Post时必须传值</param>
        /// <returns></returns>
        public static string Request(string url, MethodEnum method, string body = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                //如果是Post的话，则设置body
                if (method == MethodEnum.Post)
                {
                    byte[] requestBody = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = requestBody.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(requestBody, 0, requestBody.Length);
                }
                return Response(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回Response数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string Response(HttpWebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = stream.ReadToEnd();
                    return responseText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Tool

        /// <summary>
        /// 获取登录的UUID
        /// </summary>
        /// <returns></returns>
        public static string GetUuid(long id)
        {
            try
            {
                WebClient webClient = new WebClient { Credentials = CredentialCache.DefaultCredentials };
                Byte[] pageData = webClient.DownloadData(UrlGetUuid+id); //从指定网站下载数据
                string html = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            
                Regex reg = new Regex(@"/connect/qrcode/((?!"").)+");
                var m = reg.Match(html).Value;
                return m.Replace("/connect/qrcode/", "");
            }

            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());

            }
            return null;
        }

        /// <summary>
        /// 验证是否登录,返回Code
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static string CheckLogin(string uuid)
        {
            var url = UrlWaitLogin.Formats(uuid, TimeStamp());
            var rst = Request(url, MethodEnum.Get);
            return rst;
        }

        /// <summary>
        /// 验证code获取Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AuthCodeDto AuthCode(string code)
        {
            var url = UrlAuthCode+code;
            var rst = Request(url, MethodEnum.Get);
            var dto = JsonConvert.DeserializeObject<AuthCodeDto>(rst);
            LogHelper.Info($"AuthCode:{rst}");
            return dto;
        }

        /// <summary>
        /// 刷新身份令牌
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static string RefreshToken(string refreshToken)
        {
            var url = UrlRefreshToken.Formats(refreshToken);
            var rst = Request(url, MethodEnum.Get);
            var dto = JsonConvert.DeserializeObject<AuthCodeDto>(rst);
            LogHelper.Info($"RefreshToken:{rst}");
            return dto.AccessToken;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        public static WeChatUserInfo GetUserInfo(string accessToken, string openId)
        {
            var url = UrlUserInfo.Formats(accessToken, openId);
            var rst = Request(url, MethodEnum.Get);
            var dto = JsonConvert.DeserializeObject<WeChatUserInfo>(rst);
            LogHelper.Info($"UserInfo:{rst}");
            return dto;
        }

        #endregion
    }

    public enum MethodEnum
    {
        Post,
        Get
    }

    public class AuthCodeDto
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { set; get; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { set; get; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { set; get; }

        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { set; get; }

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { set; get; }

        /// <summary>
        /// 当且仅当该网站应用已获得该用户的userinfo授权时，才会出现该字段。
        /// </summary>
        [JsonProperty("unionid")]
        public string Unionid { set; get; }
    }

    public class WeChatUserInfo
    {
        /// <summary>
        /// 普通用户的标识，对当前开发者帐号唯一
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { set; get; }

        /// <summary>
        /// 普通用户昵称
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { set; get; }

        /// <summary>
        /// 普通用户性别，1为男性，2为女性
        /// </summary>
        [JsonProperty("sex")]
        public int Sex { set; get; }

        /// <summary>
        /// 普通用户个人资料填写的省份
        /// </summary>
        [JsonProperty("province")]
        public string Province { set; get; }

        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        [JsonProperty("city")]
        public string City { set; get; }

        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        [JsonProperty("country")]
        public string Country { set; get; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        [JsonProperty("headimgurl")]
        public string HeadImageUrl { set; get; }

        /// <summary>
        /// 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        [JsonProperty("unionid")]
        public string Unionid { set; get; }

        /// <summary>
        /// 用户特权信息，json数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        [JsonProperty("privilege")]
        public List<string> Privilege { set; get; }
    }
}
