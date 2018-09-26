using Common.Data;
using Taekwondo.Data.DTOs;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 学生课程表
	/// </summary>
	public class TrainingOrganizationClassStudentLessonMap:Entity
    {
	    /// <summary>
	    /// 课程Id
	    /// </summary>
	    public long TrainingOrganizationClassLessonId { set; get; }

	    /// <summary>
	    /// 学员Id
	    /// </summary>
	    public long GenearchChildId { set; get; }
		
	}

	public class TrainingOrganizationClassStudentLessonMapQuery : BaseQuery<TrainingOrganizationClassStudentLessonMap>
	{
	}

	public class TrainingOrganizationClassStudentLessonMapRepository : QueryRepositoryBase<TrainingOrganizationClassStudentLessonMap, TrainingOrganizationClassStudentLessonMapQuery>
	{
		public TrainingOrganizationClassStudentLessonMapRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
