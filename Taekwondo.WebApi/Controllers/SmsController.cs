using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Sms;
using Taekwondo.Data.Enums;
using Taekwondo.Service;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 账号管理
	/// </summary>
	[Route("api/[controller]")]
	public class SmsController:BaseController
	{
		private readonly SmsService _smsService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="smsService"></param>
	    public SmsController(SmsService smsService)
		{
			_smsService = smsService;
		}

		/// <summary>
		/// 发送短信
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("Send")]
		public IActionResult Post(SmsRequest request)
		{
			var code = _smsService.Send(request.Mobile, (SmsType)request.Type,Ip);
			request.ResponseModel=new SmsResponse
			{
				Code = code
			};
			return this.Success(request.ResponseModel);
		}

	}
}
