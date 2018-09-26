using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 通知
	/// </summary>
    public class Notice:Entity
    {
		/// <summary>
		/// 用户Id
		/// </summary>
	    public long UserId { get; set; }

		/// <summary>
		/// 信息内容
		/// </summary>
		public string Message { set; get; }

		/// <summary>
		/// 是否已阅
		/// </summary>
		[DefaultValue(false)]
		public bool IsRead { set; get; }
    }

	/// <summary>
	/// 
	/// </summary>
	public class NoticeQuery : BaseQuery<Notice>
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		public long? UserId { get; set; }

		/// <summary>
		/// 信息内容
		/// </summary>
		public string Message { set; get; }


	}

	/// <inheritdoc />
	/// <summary>
	/// 通知
	/// </summary>
	public class NoticeRepository : QueryRepositoryBase<Notice, NoticeQuery>
	{
		/// <inheritdoc />
		public NoticeRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<Notice, bool>> Where(NoticeQuery query)
		{
			var ex = base.Where(query);
			if (query.Message.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.Message.Contains(query.Message));
			}
			if (query.UserId.HasValue)
			{
				ex = ex.And(e => e.UserId == query.UserId);
			}
			return ex;
		}
	}
}
