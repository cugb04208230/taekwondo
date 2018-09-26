//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Text;
//using Aop.Api;
//using Aop.Api.Request;
//using Aop.Api.Response;
//using Common.Extensions;
//using Common.Log;
//using Newtonsoft.Json;
//
//namespace Common.Authorized.Login
//{
//    public static class AliPayLogin
//    {
//        private static string Scope = "auth_user,auth_base";
//
//        private static string AppId => Settings.AlipayLoginAppId;
//        private static string Getway => Settings.AlipayGetway;
//        private static string PrivateKey => Settings.AlipayLoginPrivateKey;
//        private static string PublicKey => Settings.AlipayLoginPublickKey;
//        private static string RedirectBaseUrl => Settings.AlipayAuthCallBack;
//        private static string AlipayPublicKey => Settings.AlipayLoginAlipayPublickKey;
//
//        //http://www.csgrobot.net:9001/alipay/callback
//        public static string UrlGetQrCode =
//                $"https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id={AppId}&scope={Scope}&redirect_uri={RedirectBaseUrl.UrlEncode()}&state=";
//
//        public static string UrlCheckUUid = "";
//
//        public static string UrlAuthCode =
//                "https://openapi.alipay.com/gateway.do?method=alipay.system.oauth.token&charset=utf8&sign_type=RSA2&version=1.0&" +
//                $"timestamp={0}&app_id={AppId}&sign={1}&grant_type={2}&code={3}&refresh_token={4}";
//
//        public static string UrlUserInfo =
//                "https://openapi.alipay.com/gateway.do?method=alipay.user.info.share&sign_type=RSA2&version=1.0&" +
//                $"timestamp={0}&app_id={AppId}&sign={1}&auth_token={2}";
//
//        /// <summary>
//        /// 获取UUID
//        /// </summary>
//        /// <returns></returns>
//        public static string GetUuid()
//        {
//            return string.Empty;
//        }
//
//        /// <summary>
//        /// 获取二维码
//        /// Todo
//        /// </summary>
//        /// <returns></returns>
//        public static string GetQrCode(long idenfityId)
//        {
//            return UrlGetQrCode+idenfityId;
//        }
//        
//
//        public static IDictionary<string, object> AuthCode(string code)
//        {
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json","1.0", "RSA2", AlipayPublicKey);
//            AlipaySystemOauthTokenRequest request =
//                new AlipaySystemOauthTokenRequest
//                {
//                    Code = code,
//                    GrantType = "authorization_code"
//                };
//            try
//            {
//                AlipaySystemOauthTokenResponse oauthTokenResponse = alipayClient.Execute(request);
//                IDictionary<string, object> dic = oauthTokenResponse.ToDictionary();
//                LogHelper.Info(dic);
//                return dic;
//            }
//            catch (Exception e)
//            {
//                //处理异常 Todo
//                LogHelper.Error(e);
//            }
//            return new Dictionary<string, object>();
//        }
//
//        /// <summary>
//        /// 获取阿里用户信息
//        /// </summary>
//        public static AlipayUserInfoShareResponse GetUserInfo(string token)
//        {
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayUserInfoShareRequest request = new AlipayUserInfoShareRequest();
//            try
//            {
//                AlipayUserInfoShareResponse userinfoShareResponse = alipayClient.Execute(request, token);
//                return userinfoShareResponse;
//            }
//            catch (Exception e)
//            {
//                //处理异常 Todo
//            }
//            return null;
//        }
//
//        #region Http
//        /// <summary>
//        /// 向服务器发送Request
//        /// </summary>
//        /// <param name="url">字符串</param>
//        /// <param name="method">枚举类型的方法Get或者Post</param>
//        /// <param name="body">Post时必须传值</param>
//        /// <returns></returns>
//        public static string Request(string url, MethodEnum method, string body = "")
//        {
//            try
//            {
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//                request.Method = method.ToString();
//                //如果是Post的话，则设置body
//                if (method == MethodEnum.Post)
//                {
//                    byte[] requestBody = Encoding.UTF8.GetBytes(body);
//                    request.ContentLength = requestBody.Length;
//
//                    Stream requestStream = request.GetRequestStream();
//                    requestStream.Write(requestBody, 0, requestBody.Length);
//                }
//                return Response(request);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }
//
//        /// <summary>
//        /// 返回Response数据
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        private static string Response(HttpWebRequest request)
//        {
//            try
//            {
//                using (var response = request.GetResponse())
//                using (var stream = new StreamReader(response.GetResponseStream()))
//                {
//                    var responseText = stream.ReadToEnd();
//                    return responseText;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }
//        #endregion
//
//    }
//
//    public class AliAuthCodeDto
//    {
//        [JsonProperty("alipay_system_oauth_token_response")]
//        public Response ResponseDto { set; get; }
//
//        [JsonProperty("sign")]
//        public string Sign { set; get; }
//
//        public class Response
//        {
//            /// <summary>
//            /// 支付宝用户的唯一userId
//            /// </summary>
//            [JsonProperty("user_id")]
//            public string UserId { set; get; }
//
//            /// <summary>
//            /// 访问令牌。通过该令牌调用需要授权类接口
//            /// </summary>
//            [JsonProperty("access_token")]
//            public string AccessToken { set; get; }
//
//            /// <summary>
//            /// 访问令牌的有效时间，单位是秒。
//            /// </summary>
//            [JsonProperty("expires_in")]
//            public string ExpiresIn { set; get; }
//
//            /// <summary>
//            /// 刷新令牌。通过该令牌可以刷新access_token
//            /// </summary>
//            [JsonProperty("refresh_token")]
//            public string RefreshToken { set; get; }
//
//            /// <summary>
//            /// 刷新令牌的有效时间，单位是秒。
//            /// </summary>
//            [JsonProperty("re_expires_in")]
//            public string ReExpiresIn { set; get; }
//        }
//
//        [JsonProperty("error_response")]
//        public ErrorResponse ErrorResponse { set; get; }
//    }
//
//
//    public class ErrorResponse
//    {
//
//        [JsonProperty("code")]
//        public string Code { set; get; }
//        [JsonProperty("msg")]
//        public string Message { set; get; }
//
//        /// <summary>
//        /// isv.grant-type-invalid	grant_type参数不正确	grant_type必须是authorization_code、refresh_token二者之一 若传入authorization_code为code换取令牌，若传入refresh_token为刷新令牌
//        ///isv.code-invalid 授权码(auth_code) <br>错误、状态不对或过期 使用有效的auth_code重新执行令牌换取，或引导用户重新授权
//        ///isv.refresh-token-invalid 刷新令牌(refresh_token) 错误或状态不对  使用有效的refresh_token重新执行令牌刷新，或引导用户重新授权
//        ///isv.refresh-token-time-out 刷新令牌(refresh_token) 过期   使用有效的refresh_token重新执行令牌刷新，或引导用户重新授权
//        ///isv.refreshed-token-invalid 刷新出来的令牌无效   使用返回的刷新令牌再次刷新
//        ///isv.invalid-app-id 调用接口的应用标识(app_id) 与令牌授权的应用不相符    传入正确的app_id和令牌，若开发者支付宝账号名下有多个app_id，或者开发者管理多个归属于不同支付宝账号的app_id，请注意不要混用不同app_id的code
//        ///isp.unknow-error 未知错误    重试，或联系支付宝客服
//        ///</summary>
//        [JsonProperty("sub_code")]
//        public string SubCode { set; get; }
//        [JsonProperty("sub_msg")]
//        public string SubMessage { set; get; }
//    }
//
//    public class AliUserInfo
//    {
//        [JsonProperty("error_response")]
//        public ErrorResponse ErrorResponse { set; get; }
//
//        public class Response
//        {
//
//            [JsonProperty("code")]
//            public string Code { set; get; }
//            [JsonProperty("msg")]
//            public string Message { set; get; }
//
//            /// <summary>
//            /// 支付宝用户的唯一userId
//            /// </summary>
//            [JsonProperty("user_id")]
//            public string UserId { set; get; }
//
//            [JsonProperty("avatar")]
//            public string Avatar { set; get; }
//
//            [JsonProperty("province")]
//            public string Province { set; get; }
//
//            [JsonProperty("city")]
//            public string City { set; get; }
//
//            [JsonProperty("nick_name")]
//            public string NickName { set; get; }
//
//            [JsonProperty("is_student_certified")]
//            public string IsStudentCertified { set; get; }
//
//            [JsonProperty("user_type")]
//            public string UserType { set; get; }
//
//            [JsonProperty("user_status")]
//            public string UserStatus { set; get; }
//
//            [JsonProperty("is_certified")]
//            public string IsCertified { set; get; }
//
//            [JsonProperty("gender")]
//            public string Gender { set; get; }
//
//        }
//    }
//}
