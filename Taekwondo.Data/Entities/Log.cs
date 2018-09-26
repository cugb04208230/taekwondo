using System;
using Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Taekwondo.Data.Entities
{
	/// <summary>
	/// 日志
	/// </summary>
    public class Log
    {
		/// <summary>
		/// 主键
		/// </summary>
	    public long Id { get; set; }
		/// <summary>
		/// 应用
		/// </summary>
	    public string Application { get; set; }
		/// <summary>
		/// 记录时间
		/// </summary>
	    public DateTime Logged { get; set; }
		/// <summary>
		/// 记录等级
		/// Info,Error
		/// </summary>
	    public string Level { get; set; }
		/// <summary>
		/// 信息
		/// </summary>
	    public string Message { get; set; }
		/// <summary>
		/// 日志位置表示
		/// </summary>
	    public string Logger { get; set; }
		/// <summary>
		/// 调用点
		/// </summary>
	    public string Callsite { get; set; }
		/// <summary>
		/// 异常
		/// </summary>
	    public string Exception { get; set; }
	}


	/// <summary>
	/// 
	/// </summary>
	public class LogQuery:BaseQuery<Log>
	{
		/// <summary>
		/// 应用
		/// </summary>
		public string Application { get; set; }
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime? Logged { get; set; }
		/// <summary>
		/// 日志等级
		/// </summary>
		public string Level { get; set; }
		/// <summary>
		/// 日志内容
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// 日志记录位置
		/// </summary>
		public string Logger { get; set; }
		/// <summary>
		/// 调用点
		/// </summary>
		public string Callsite { get; set; }
		/// <summary>
		/// 异常
		/// </summary>
		public string Exception { get; set; }
	}

//	/// <inheritdoc />
//	/// <summary>
//	/// </summary>
//	public class LogRepository : QueryRepositoryBase<Log, LogQuery>
//	{
//		/// <inheritdoc />
//		public LogRepository(DbContext dbContext) : base(dbContext)
//		{
//		}
//	}
}
