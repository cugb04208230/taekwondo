using System.ComponentModel;
using Common.Data;

namespace Taekwondo.Data.DTOs.Account
{
	/// <inheritdoc />
	/// <summary>
	/// 登录请求
	/// </summary>
    public class LoginRequest:BaseRequest<LoginResponse>
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
	}

	/// <summary>
	/// 登陆结果
	/// </summary>
	public class LoginResponse
	{
		/// <summary>
		/// 用户信息
		/// </summary>
		[Description("用户信息")]
		public UserInfo UserInfo { set; get; }

		/// <summary>
		/// 授权票据
		/// </summary>
		public string Token { set; get; }
	}
}
