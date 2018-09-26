using System.ComponentModel;
using Common.Data;

namespace Taekwondo.Data.DTOs.Account
{
	/// <inheritdoc />
	/// <summary>
	/// 找回密码请求
	/// </summary>
    public class ResetPasswordRequest:BaseRequest<ResetPasswordResponse>
	{
		/// <summary>
		/// 手机号
		/// </summary>
		[DtoValidate(Regex = "^1[\\d]{10}$")]
		[Description("手机号")]
		public string Mobile { set; get; }

		/// <summary>
		/// 密码
		/// </summary>
		[DtoValidate(MaxLength = 20, MinLength = 6, Regex = "^[A-Za-z0-9]{6,20}$", RegexNotice = "密码仅支持只支持英文大小写字母、阿拉伯数字")]
		[Description("密码")]
		public string Password { set; get; }

		/// <summary>
		/// 验证码
		/// </summary>
		[DtoValidate(Regex = "^[\\d]{4,6}$")]
		[Description("验证码")]
		public string Code { set; get; }
	}

	/// <summary>
	/// 找回密码请求结果
	/// </summary>
	public class ResetPasswordResponse
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
