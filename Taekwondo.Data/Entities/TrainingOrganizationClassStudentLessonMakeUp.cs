using Common.Data;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 学员补课申请
	/// </summary>
    public class TrainingOrganizationClassStudentLessonMakeUp:Entity
    {

	    /// <summary>
	    /// 家长Id
	    /// </summary>
	    public long GenearchId { get; set; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long GenearchChildId { set; get; }

		/// <summary>
	    /// 课程Id
	    /// </summary>
	    public long TrainingOrganizationClassLessonId { set; get; }

		/// <summary>
		/// 申请同意后回存的MapId
		/// </summary>
		public long StudentLessonMapId { set; get; }

		/// <summary>
		/// 原始课程映射Id
		/// </summary>
		public long OriginalStudentLessonMapId { set; get; }

		/// <summary>
		/// 申请状态
		/// </summary>
		public StudentLessonMakeUpStatus Status { set; get; }

		/// <summary>
		/// 备注原因
		/// </summary>
		public string Remark { set; get; }

	}

	public class
		TrainingOrganizationClassStudentLessonMakeUpQuery : BaseQuery<TrainingOrganizationClassStudentLessonMakeUp>
	{
	}

	public class TrainingOrganizationClassStudentLessonMakeUpRepository : QueryRepositoryBase<
		TrainingOrganizationClassStudentLessonMakeUp, TrainingOrganizationClassStudentLessonMakeUpQuery>
	{
		public TrainingOrganizationClassStudentLessonMakeUpRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
