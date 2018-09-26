using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 场馆班级老师映射关系
	/// </summary>
    public class TrainingOrganizationClassTeacherMap:Entity
    {
		/// <summary>
		/// 班级Id
		/// </summary>
		public long TrainingOrganizationClassId { set; get; }

		/// <summary>
		/// 老师Id
		/// </summary>
		public long TrainingOrganizationTeacherId { set; get; }

		/// <summary>
		/// 老师类型
		/// <see cref="TeacherType"/>
		/// </summary>
		public TeacherType TeacherInClassType { set; get; }
	}

	public class TrainingOrganizationClassTeacherMapQuery:BaseQuery<TrainingOrganizationClassTeacherMap>
	{
	}

	public class TrainingOrganizationClassTeacherMapRepository : QueryRepositoryBase<TrainingOrganizationClassTeacherMap,
		TrainingOrganizationClassTeacherMapQuery>
	{
		public TrainingOrganizationClassTeacherMapRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}

	public enum TeacherType
	{
		/// <summary>
		/// 班主任
		/// </summary>
		Headmaster=1,
		/// <summary>
		/// 普通老师
		/// </summary>
		Normalmaster=2
	}
}
