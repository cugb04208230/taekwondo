using System;
using System.Collections.Generic;
using Taekwondo.Data.DTOs.Teacher;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <summary>
	/// 作业输出模型
	/// </summary>
	public class HomeworkDto
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
		/// 作业标题
		/// </summary>
		public string Title { set; get; }

		/// <summary>
		/// 作业文字说明
		/// </summary>
		public string Summary { get; set; }
		/// <summary>
		/// 作业视频说明
		/// </summary>
		public List<string> Files { get; set; }
		/// <summary>
		/// 封面图片
		/// </summary>
		public List<string> Images { set; get; }

		/// <summary>
		/// 答题列表（默认10条，答题时间正序）
		/// </summary>
		public List<HomeworkAnswerDto> Answers { get; set; }

		/// <summary>
		/// 老师
		/// </summary>
		public TeacherDto Teacher { set; get; }

		/// <summary>
		/// 是否已完成
		/// </summary>
		public bool IsAnswered { set; get; }

		/// <summary>
		/// 查询对象为家长时返回
		/// <see cref="TrainingOrganizationClassHomeworkAnswerStatus"/>
		/// 1.未提交，2.已提交，3.已批阅
		/// </summary>
		public int Status { set; get; }

		/// <summary>
		/// 如果已批阅 
		/// </summary>
		public int Stars { get; set; }
	}
}
