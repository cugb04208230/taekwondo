using System;
using System.Collections.Generic;
using System.Text;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Net.SDK.Core;
using Aliyun.Net.SDK.Core.Exceptions;
using Aliyun.Net.SDK.Core.Http;
using Aliyun.Net.SDK.Core.Profile;
using Common.Extensions;
using Common.Log;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace Common.Util
{
    public static class SmsHelper
    {
        public static void Send(List<string> mobiles,string msg)
        {
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            String accessKeyId = "LTAIuVzMCsSxgtAp";//你的accessKeyId
            String accessKeySecret = "AAAXP4QiIVEzZIyI6lsgG6V96p7AlR";//你的accessKeySecret

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest {Method = MethodType.POST};
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mobiles.Join(",");
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "科大智能";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = "SMS_99360068";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\""+msg+"\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                LogHelper.Init("Sms").Error(e);
            }
            catch (ClientException e)
            {
                LogHelper.Init("Sms").Error(e);
            }
        }
    }
}
