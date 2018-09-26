using System;
using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 课表信息
	/// </summary>
    public class ClassTimetable : Entity
	{
		/// <summary>
		/// 俱乐部Id
		/// </summary>
		public long EntId { set; get; }

		/// <summary>
		/// 培训机构Id
		/// </summary>
		public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }

		/// <summary>
		/// 开始时间 小时-24H
		/// </summary>
		public string StartHour { set; get; }

		/// <summary>
		/// 开始时间 分钟-60
		/// </summary>
		public string StartMinute { set; get; }

		/// <summary>
		/// 结束时间 小时-24H
		/// </summary>
		public string EndHour { set; get; }

		/// <summary>
		/// 结束时间 分钟-60
		/// </summary>
		public string EndMinute { set; get; }

		/// <summary>
		/// 星期
		/// </summary>
		public DayOfWeek DayOfWeek { set; get; }

	}
}
