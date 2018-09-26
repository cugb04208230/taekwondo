using System;
using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 短信验证码
	/// </summary>
    public class VerifyCode:Entity
    {
		/// <summary>
		/// 验证码
		/// </summary>
	    public string Code { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 验证码类型
		/// <see cref="Enums.SmsType"/>
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// 请求发起的地址
		/// </summary>
		public string Ip { set; get; }

		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime ExpriedAt { set; get; }
    }

	/// <summary>
	/// 验证码查询
	/// </summary>
	public class VerifyCodeQuery : BaseQuery<VerifyCode>
	{

		/// <summary>
		/// 验证码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 验证码类型
		/// <see cref="Enums.SmsType"/>
		/// </summary>
		public int? Type { get; set; }
	}


	/// <inheritdoc />
	public class VerifyCodeRepository : QueryRepositoryBase<VerifyCode, VerifyCodeQuery>
	{
		/// <inheritdoc />
		public VerifyCodeRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}

}
