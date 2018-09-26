using System.ComponentModel;
using Common.Data;

namespace Taekwondo.Data.DTOs.Account
{
	/// <summary>
	/// 注册请求
	/// </summary>
    public class RegisterRequest:BaseRequest<RegisterResponse>
	{
		/// <summary>
		/// 手机号
		/// </summary>
		[Description("手机号")]
		[DtoValidate(Regex = "^1[\\d]{10}$")]
		public string Mobile { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Description("密码")]
		[DtoValidate(Regex = "^[\\S]{6,20}$")]
		public string Password { set; get; }

		/// <summary>
		/// 验证码
		/// </summary>
		[Description("验证码")]
		[DtoValidate(Regex = "^[\\d]{4,6}$")]
		public string Code { set; get; }
	}

	/// <summary>
	/// 注册请求结果
	/// </summary>
	public class RegisterResponse
	{
		/// <summary>
		/// 用户信息
		/// </summary>
		[Description("用户信息")]
		public UserInfo UserInfo { set; get; }
		/// <summary>
		/// 找回密码后免登录，返回登录票据
		/// </summary>
		public string Token { set; get; }
	}
}
