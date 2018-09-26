using System;
using System.Collections.Generic;
using System.Text;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <inheritdoc />
	/// <summary>
	/// 作业批阅
	/// </summary>
    public class MarkingRequest:BaseRequest<MarkingResponse>
	{
		/// <summary>
		/// 答题记录Id
		/// </summary>
		public long AnswerId { get; set; }
		/// <summary>
		/// 批阅文字说明
		/// </summary>
		public string ReadoverText { get; set; }

		/// <summary>
		/// 评分
		/// </summary>
		public int? Stars { set; get; }
	}
	/// <summary>
	/// 作业批阅结果
	/// </summary>
	public class MarkingResponse
	{
	}
}
