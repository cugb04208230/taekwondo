using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 教师履历
	/// </summary>
    public class TrainingOrganizationTeacherCurriculumVitae:Entity
    {
    }

	/// <summary>
	/// 阅卷查询
	/// </summary>
	public class TrainingOrganizationTeacherCurriculumVitaeQuery : BaseQuery<TrainingOrganizationTeacherCurriculumVitae>
	{
	}

	/// <inheritdoc />
	/// <summary>
	/// 阅卷
	/// </summary>
	public class TrainingOrganizationTeacherCurriculumVitaeRepository : QueryRepositoryBase<
		TrainingOrganizationTeacherCurriculumVitae, TrainingOrganizationTeacherCurriculumVitaeQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationTeacherCurriculumVitaeRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
