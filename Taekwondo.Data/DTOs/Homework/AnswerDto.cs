using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <inheritdoc />
	/// <summary>
	/// 家长提交答案
	/// </summary>
    public class AnswerRequest:BaseRequest<AnswerResponse>
    {
		/// <summary>
		/// 作业Id
		/// </summary>
	    public long HomeworkId { get; set; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long StudentId { set; get; }

	    /// <summary>
	    /// 作业答题文字说明
	    /// </summary>
	    public string Summary { get; set; }

	    /// <summary>
	    /// 文件列表(文件名称，以逗号隔开)
	    /// </summary>
	    public string Files { set; get; }


		/// <summary>
		/// 封面图片
		/// </summary>
		public string Images { set; get; }
	}
	/// <summary>
	/// 提交答案结果
	/// </summary>
	public class AnswerResponse
	{
	}
}
