using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 登陆日志
	/// </summary>
    public class LoginLog:Entity
    {
		/// <summary>
		/// 登陆票据
		/// </summary>
		public string Token { set; get; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public long UserId { set; get; }

		/// <summary>
		/// 过期时间
		/// </summary>
	    public DateTime ExpiredAt { get; set; }

		/// <summary>
		/// 登陆Ip
		/// </summary>
		public string Ip { set; get; }

    }

	/// <summary>
	/// 登录记录查询
	/// </summary>
	public class LoginLogQuery : BaseQuery<LoginLog>
	{
		/// <summary>
		/// 用户
		/// </summary>
		public long? UserId { set; get; }
		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }
	}


	/// <inheritdoc />
	/// <summary>
	/// 仓储 登录日志
	/// </summary>
	public class LoginLogRepository : QueryRepositoryBase<LoginLog, LoginLogQuery>
	{
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="dbContext"></param>
		public LoginLogRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<LoginLog, bool>> Where(LoginLogQuery query)
		{
			var ex = base.Where(query);
			if (query.UserId.HasValue)
			{
				ex = ex.And(e => e.UserId == query.UserId);
			}
			if (!query.Mobile.IsNullOrEmpty())
			{
				var ids = DbContext.Set<UserAccount>().Where(e => e.Mobile.Contains(query.Mobile)).Select(e => e.Id);
				ex = ex.And(e => ids.Contains(e.UserId));
			}
			return ex;
		}
	}
}
