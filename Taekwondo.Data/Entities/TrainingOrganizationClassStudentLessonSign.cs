using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 课程签到
	/// </summary>
    public class TrainingOrganizationClassStudentLessonSign:Entity
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
	    public long GenearchChild { set; get; }
	}

	public class TrainingOrganizationClassLessonSignQuery : BaseQuery<TrainingOrganizationClassStudentLessonSign>
	{
	}

	public class TrainingOrganizationClassLessonSignRepository : QueryRepositoryBase<TrainingOrganizationClassStudentLessonSign,
		TrainingOrganizationClassLessonSignQuery>
	{
		public TrainingOrganizationClassLessonSignRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
