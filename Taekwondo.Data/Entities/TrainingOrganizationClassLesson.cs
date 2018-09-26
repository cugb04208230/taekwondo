using System;
using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 班级课程
	/// </summary>
    public class TrainingOrganizationClassLesson:Entity
    {
		/// <summary>
		/// 班级编号
		/// </summary>
	    public long TrainingOrganizationClassId { get; set; }

		/// <summary>
		/// 课程名称
		/// </summary>
		public string LessonName { set; get; }

		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { set; get; }
		
    }

	public class TrainingOrganizationClassLessonQuery : BaseQuery<TrainingOrganizationClassLesson>
	{
	}

	public class TrainingOrganizationClassLessonRepository : QueryRepositoryBase<TrainingOrganizationClassLesson,
		TrainingOrganizationClassLessonQuery>
	{
		public TrainingOrganizationClassLessonRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}

}
