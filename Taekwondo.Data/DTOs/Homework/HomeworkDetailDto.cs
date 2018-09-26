using System.Collections.Generic;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <inheritdoc />
	/// <summary>
	/// 家庭作业详情获取
	/// </summary>
    public class HomeworkDetailRequest:BaseRequest<HomeworkDetailResponse>
    {
		/// <summary>
		/// 作业Id
		/// </summary>
	    public long HomeworkId { get; set; }

		/// <summary>
		/// 学员Id,选填选项，如果当前查询对象是学生家长，则为必填，如果查询对象是老师，则不必填写
		/// </summary>
		public long? StudentId { set; get; }
    }

	/// <summary>
	/// 家庭作业详情获取结果
	/// </summary>
	public class HomeworkDetailResponse
	{
		/// <summary>
		/// 作业
		/// </summary>
		public HomeworkDto Homework { get; set; }
		
		/// <summary>
		/// 是否允许继续答题
		/// </summary>
		public bool CanAnswer { set; get; }
	}
}
