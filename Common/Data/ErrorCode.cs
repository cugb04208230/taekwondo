using System.ComponentModel;

namespace Common.Data
{
    public enum ErrorCode
	{
		/// <summary>
		/// 通用错误
		/// </summary>
		[Description("通用错误")]
		GeneralError = -1,

		/// <summary>
		/// 成功
		/// </summary>
		[Description("成功")]
		Success = 0,

		/// <summary>
		/// 系统错误
		/// </summary>
		[Description("系统错误")]
		SystemError = 1,

		/// <summary>
		/// 请求数据格式有误
		/// </summary>
		[Description("请求数据格式有误")]
		ErrorParameterFormat = 1001,

		/// <summary>
		/// 错误的参数
		/// </summary>
		[Description("错误的参数")]
	    ErrorParameter = 1002,

		/// <summary>
		/// 无效的票据，请重新登录
		/// </summary>
		[Description("无效的票据，请重新登录")]
		InvalidToken =1003,

		/// <summary>
		/// 该手机号已注册
		/// </summary>
		[Description("该手机号已注册")]
		RegisterMobileIsExisted =1004,

		/// <summary>
		/// 错误的验证码
		/// </summary>
		[Description("错误的验证码")]
		ErrorVerifycode = 1005,

		/// <summary>
		/// 手机号或者密码错误
		/// </summary>
		[Description("手机号或者密码错误")]
		ErrorMobileOrPassword = 1006,

		/// <summary>
		/// 改账户已经停用，请与管理员联系
		/// </summary>
		[Description("改账户已经停用，请与管理员联系")]
		AccountIsStop = 1007,

		/// <summary>
		/// 该手机号不存在，请前往注册
		/// </summary>
		[Description("该手机号不存在，请前往注册")]
		MobileIsNotExisted = 1008
	}
}
