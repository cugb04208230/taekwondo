using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 作业审批
	/// </summary>
    public class TrainingOrganizationClassHomeworkMarking:Entity
    {
		/// <summary>
		/// 教师Id
		/// </summary>
	    public long TeacherId { get; set; }

	    /// <summary>
	    /// 作业Id
	    /// </summary>
	    public long TrainingOrganizationClassHomeworkId { get; set; }

	    /// <summary>
	    /// 家长Id
	    /// </summary>
	    public long GenearchId { set; get; }

	    /// <summary>
	    /// 学员Id
	    /// </summary>
	    public long GenearchChildId { set; get; }

	}

	public class TrainingOrganizationClassHomeworkMarkingQuery : BaseQuery<TrainingOrganizationClassHomeworkMarking>
	{
	}

	public class TrainingOrganizationClassHomeworkMarkingRepository : QueryRepositoryBase<
		TrainingOrganizationClassHomeworkMarking, TrainingOrganizationClassHomeworkMarkingQuery>
	{
		public TrainingOrganizationClassHomeworkMarkingRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
