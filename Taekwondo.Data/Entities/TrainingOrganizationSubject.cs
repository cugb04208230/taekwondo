using System.ComponentModel.DataAnnotations.Schema;
using Common.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构科目
    /// </summary>
    public class TrainingOrganizationSubject : Entity
    {
        /// <summary>
        /// 培训机构Id
        /// </summary>
        public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 培训机构科目名称
		/// </summary>
		[JsonProperty("name")]
		public string TrainingOrganizationSubjectName { set; get; }

		#region Model

		/// <summary>
		/// 培训机构
		/// </summary>
	    [NotMapped]
		public TrainingOrganization TrainingOrganization { set; get; }

        #endregion
    }

	/// <summary>
	/// 科目查询
	/// </summary>
	public class TrainingOrganizationSubjectQuery:BaseQuery<TrainingOrganizationSubject>
	{

	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationSubjectRepository : QueryRepositoryBase<TrainingOrganizationSubject,
			TrainingOrganizationSubjectQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationSubjectRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
