using System;
using Common.Data;

namespace Taekwondo.Data.DTOs.Notice
{
	/// <inheritdoc />
	/// <summary>
	/// 获取消息通知
	/// </summary>
    public class NoticeQueryRequest:BasePageRequest<NoticeQueryResponse>
    {
    }

	/// <summary>
	/// 获取消息通知结果
	/// </summary>
	public class NoticeQueryResponse : QueryResult<Entities.Notice>
	{
	}

	/// <summary>
	/// 通知模型
	/// </summary>
	public class NoticeDto
	{
		/// <summary>
		/// 标识
		/// </summary>
		public virtual long Id { get; set; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public long UserId { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreatedAt { get; set; }
		/// <summary>
		/// 信息内容
		/// </summary>
		public string Message { set; get; }

		/// <summary>
		/// 是否已阅
		/// </summary>
		public bool IsRead { set; get; }

		/// <summary>
		/// 头像
		/// </summary>
		public string HeadPic { set; get; }


		/// <summary>
		/// 标题
		/// </summary>
		public string Title { set; get; }
	}
}
