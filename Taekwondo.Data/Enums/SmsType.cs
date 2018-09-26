using System.ComponentModel;

namespace Taekwondo.Data.Enums
{
	/// <summary>
	/// 短信类型
	/// </summary>
    public enum SmsType
    {
		/// <summary>
		/// 注册
		/// </summary>
		[Description("注册")]
		Register=1,
		/// <summary>
		/// 
		/// </summary>
	    [Description("找回密码")]
	    ResetPassword = 2
    }


	/// <summary>
	/// 短信发送状态
	/// </summary>
	public enum SmsStatus
	{
		/// <summary>
		/// 初始
		/// </summary>
		Original = 1,
		/// <summary>
		/// 成功
		/// </summary>
		Success = 2,
		/// <summary>
		/// 失败
		/// </summary>
		Fail = 3
	}
}
