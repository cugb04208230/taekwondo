using System.ComponentModel;
using Common.Data;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Sms
{
	/// <inheritdoc />
	/// <summary>
	/// 短信发送请求
	/// </summary>
    public class SmsRequest: BaseRequest<SmsResponse>
	{

		/// <summary>
		/// 手机号
		/// </summary>
		[Description("手机号")]
		[DtoValidate(Regex = "^1[\\d]{10}$")]
		public string Mobile { get; set; }

		/// <summary>
		/// 短信类型
		/// 1.注册,
		/// 2.找回密码 
		/// </summary>
		[Description("短信类型")]
		[DtoValidate(EnumType = typeof(SmsType) )]
		public int Type { get; set; }
	}

	/// <summary>
	/// 短信发送请求结果
	/// </summary>
	public class SmsResponse
	{
		/// <summary>
		/// 短信验证码
		/// </summary>
		public string Code { get; set; }
	}
}
