using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;

namespace Common.QCloudSms
{
	class Demo
	{
		public int result { get; set; }
		public string errMsg { get; set; }
		public string ext { get; set; }
	}

	class SmsSDKDemo
	{
		static void Main(string[] args)
		{
			// 请根据实际 appid 和 appkey 进行开发，以下只作为演示 sdk 使用
			// appid,appkey,templId申请方式可参考接入指南 https://www.qcloud.com/document/product/382/3785#5-.E7.9F.AD.E4.BF.A1.E5.86.85.E5.AE.B9.E9.85.8D.E7.BD.AE
			int sdkappid = 1400057352;
			string appkey = "5b816b513da10d9ba79543924981714c";
			string phoneNumber1 = "12345678901";
			string phoneNumber2 = "12345678902";
			string phoneNumber3 = "12345678903";
			int tmplId = 7839;

			try
			{
				SmsSingleSenderResult singleResult;
				SmsSingleSender singleSender = new SmsSingleSender(sdkappid, appkey);

				singleResult = singleSender.Send(0, "86", phoneNumber2, "测试短信，普通单发，深圳，小明，上学。", "", "");
				Console.WriteLine(singleResult);

				List<string> templParams = new List<string>();
				templParams.Add("指定模板单发");
				templParams.Add("深圳");
				templParams.Add("小明");
				// 指定模板单发
				// 假设短信模板内容为：测试短信，{1}，{2}，{3}，上学。
				singleResult = singleSender.SendWithParam("86", phoneNumber2, tmplId, templParams, "", "", "");
				Console.WriteLine(singleResult);

				SmsMultiSenderResult multiResult;
				SmsMultiSender multiSender = new SmsMultiSender(sdkappid, appkey);
				List<string> phoneNumbers = new List<string>();
				phoneNumbers.Add(phoneNumber1);
				phoneNumbers.Add(phoneNumber2);
				phoneNumbers.Add(phoneNumber3);

				// 普通群发
				// 下面是 3 个假设的号码
				multiResult = multiSender.Send(0, "86", phoneNumbers, "测试短信，普通群发，深圳，小明，上学。", "", "");
				Console.WriteLine(multiResult);

				// 指定模板群发
				// 假设短信模板内容为：测试短信，{1}，{2}，{3}，上学。
				templParams.Clear();
				templParams.Add("指定模板群发");
				templParams.Add("深圳");
				templParams.Add("小明");
				multiResult = multiSender.SendWithParam("86", phoneNumbers, tmplId, templParams, "", "", "");
				Console.WriteLine(multiResult);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}

	public static class QCloudSmsTool
	{
		public static int Sdkappid { get; } = 1400057352;
		public static string Appkey { get; } = "5b816b513da10d9ba79543924981714c";

		public static string FrogetPasswordTemp = "【花郎录】验证码{0}，您正在修改花郎录登录密码";

		public static void SendSms(string mobile, string content)
		{

			try
			{
				SmsSingleSender singleSender = new SmsSingleSender(Sdkappid, Appkey);
				SmsSingleSenderResult singleResult = singleSender.SendWithParam("86",mobile, 72637,new List<string>{content},"","","" );
			}
			catch (Exception e)
			{
				throw new PlatformException(e.Message);
			}
		}
	}
}
