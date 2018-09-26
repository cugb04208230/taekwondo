using Common.Data;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 课程请假
	/// </summary>
    public class TrainingOrganizationClassStudentLessonLeave:Entity
    {
		/// <summary>
		/// 学生课程映射Id
		/// </summary>
		public long StudentLessonMapId { set; get; }

		/// <summary>
		/// 课程Id
		/// </summary>
		public long TrainingOrganizationClassLessonId { set; get; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long GenearchChildId { set; get; }

		/// <summary>
		/// 请假原因
		/// </summary>
		public string Reason { set; get; }

		/// <summary>
		/// 是否同意
		/// </summary>
		public bool IsAgreed { set; get; }

		/// <summary>
		/// 状态
		/// </summary>
		public StudentLessonLeaveStatus Status { set; get; }

		/// <summary>
		/// 审核人Id
		/// </summary>
		public long? AuditorId { set; get; }
	}

	public class TrainingOrganizationClassLessonLeaveQuery : BaseQuery<TrainingOrganizationClassStudentLessonLeave>
	{
	}

	public class TrainingOrganizationClassLessonLeaveRepository : QueryRepositoryBase<TrainingOrganizationClassStudentLessonLeave,
		TrainingOrganizationClassLessonLeaveQuery>
	{
		public TrainingOrganizationClassLessonLeaveRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
