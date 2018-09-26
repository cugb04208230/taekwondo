using System;
using System.Collections.Generic;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Class
{
	/// <inheritdoc />
	/// <summary>
	/// 班级新增
	/// </summary>
    public class TrainingOrganizationClassAddRequest:BaseRequest<TrainingOrganizationClassAddResponse>
    {
		/// <summary>
		/// 班级名称
		/// </summary>
	    public string Name { get; set; }

		/// <summary>
		/// 老师的Id
		/// </summary>
		public long[] TeacherIds { set; get; }
	    /// <summary>
	    /// 场馆Id
	    /// </summary>
	    public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }

		/// <summary>
		/// 上课时间
		/// </summary>
		public List<TrainingOrganizationClassTime> Times { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// 班级更新
	/// </summary>
	public class TrainingOrganizationClassUpdateRequest : BaseRequest<TrainingOrganizationClassUpdateResponse>
	{

		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }

		/// <summary>
		/// 班级名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 老师的Id
		/// </summary>
		public long[] TeacherIds { set; get; }
		/// <summary>
		/// 场馆Id
		/// </summary>
		public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }
	}

	public class TrainingOrganizationClassTime
	{
		/// <summary>
		/// 开始时间
		/// </summary>
		public string StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public string EndTime { set; get; }

		/// <summary>
		/// 星期
		/// </summary>
		public DayOfWeek DayOfWeek { set; get; }
	}

	/// <summary>
	/// 班级新增结果
	/// </summary>
	public class TrainingOrganizationClassAddResponse
	{
	}
	/// <summary>
	/// 班级修改结果
	/// </summary>
	public class TrainingOrganizationClassUpdateResponse
	{
	}
}
