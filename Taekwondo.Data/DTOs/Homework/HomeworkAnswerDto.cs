using System;
using System.Collections.Generic;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <summary>
	/// 答题结果模型
	/// </summary>
	public class HomeworkAnswerDto
	{
		/// <summary>
		/// 标识
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreatedAt { set; get; }
		/// <summary>
		/// 作业Id
		/// </summary>
		public long HomeworkId { get; set; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 学员名称
		/// </summary>
		public string StudentName { set; get; }

		/// <summary>
		/// 作业答题文字说明
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// 作业答题视频说明
		/// </summary>
		public List<string> Files { get; set; }
		/// <summary>
		/// 封面图片
		/// </summary>
		public List<string> Images { set; get; }

		/// <summary>
		/// 批阅状态
		/// </summary>
		public bool Readovered { set; get; }

		/// <summary>
		/// 批阅文字说明
		/// </summary>
		public string ReadoverText { get; set; }

		/// <summary>
		/// 评分
		/// </summary>
		public int Stars { set; get; }
		/// <summary>
		/// 头像
		/// </summary>
		public string HeadPic { set; get; }
	}
}
