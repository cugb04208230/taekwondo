using System;
using System.Collections.Generic;
using Common.Extensions;
using Common.Models;
using Common.QCloudSms;
using Common.Util;
using NLog;
using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
using Taekwondo.Data;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class SmsService: BaseService
    {
	    private readonly VerifyCodeRepository _verifyCodeRepository;
	    private readonly SmsLogRepository _repository;
	    private readonly UserAccountRepository _userAccountRepository;
		public SmsService(UserAccountRepository userAccountRepository,VerifyCodeRepository verifyCodeRepository,DataBaseContext dbContext, SmsLogRepository repository) : base(dbContext)
		{
			_verifyCodeRepository = verifyCodeRepository;
			_repository = repository;
			_userAccountRepository = userAccountRepository;
		}

		/// <summary>
		/// 发短信
		/// </summary>
		/// <param name="mobile"></param>
		/// <param name="type"></param>
		/// <param name="ip"></param>
		public string Send(string mobile,SmsType type,string ip="")
		{
			var code = RandomCode.Code(6);
			_verifyCodeRepository.Insert(new VerifyCode {Code = code, Ip = ip, Mobile = mobile, Type = (int) type,ExpriedAt = DateTime.Now.AddMinutes(5)});
			var message = string.Format(QCloudSmsTool.FrogetPasswordTemp,code);
			switch (type)
			{
				case SmsType.Register:
					break;
				case SmsType.ResetPassword:
					if (!_userAccountRepository.Any(e => e.Mobile == mobile))
					{
						throw new PlatformException("该手机号尚未注册");
					}
					QCloudSmsTool.SendSms(mobile, code);
					break;
			}
			_repository.Insert(new SmsLog
		    {
			    Mobile = mobile,
			    Message = message,
			    Type = (int) type,
				Ip = ip
		    });
			return code;
		}

		#region QSms

		// 短信应用SDK AppID
	    private readonly int appid = 1400057352;
		// 短信应用SDK AppKey
	    private readonly string appkey = "5b816b513da10d9ba79543924981714c";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mobiles"></param>
		/// <param name="templateId"></param>
		/// <param name="items"></param>
		private void SendQSmsMessage(List<string> mobiles,int templateId,params string[] items)
	    {
		    try
		    {
			    var message = "";

			    SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
			    var result = ssender.send(0, "86", mobiles[0],message, "", "");
			    LogManager.GetLogger("QSms").Info(result.SerializeObject());
			}
		    catch (JSONException e)
		    {
				LogManager.GetLogger("QSms").Error(e);
		    }
		    catch (HTTPException e)
			{
				LogManager.GetLogger("QSms").Error(e);
			}
		    catch (Exception e)
			{
				LogManager.GetLogger("QSms").Error(e);
			}
		}

	    #endregion

	}
}
