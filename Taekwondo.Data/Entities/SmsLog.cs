using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 短信记录
	/// </summary>
	public class SmsLog:Entity	
    {
		/// <summary>
		/// 手机号
		/// </summary>
	    public string Mobile { get; set; }

		/// <summary>
		/// 短信内容
		/// </summary>
	    public string Message { get; set; }

		/// <summary>
		/// 短信类型
		/// <see cref="Enums.SmsType"/>
		/// </summary>
	    public int Type { get; set; }

		/// <summary>
		/// 短信Ip	
		/// </summary>
	    public string Ip { get; set; }
    }

	/// <summary>
	/// 短信记录查询
	/// </summary>
	public class SmsLogQuery : BaseQuery<SmsLog>
	{
		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 短信内容
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 短信类型
		/// </summary>
		public int? Type { get; set; }
	}


	/// <inheritdoc />
	public class SmsLogRepository : QueryRepositoryBase<SmsLog, SmsLogQuery>
	{
		/// <inheritdoc />
		public SmsLogRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
