using Common.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构奖品
    /// </summary>
    public class TrainingOrganizationPrize:Entity
    {
        /// <summary>
        /// 培训机构Id
        /// </summary>
        public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 奖品名称
		/// </summary>
		[JsonProperty("name")]
		public string TrainingOrganizationPrizeName { set; get; }

        /// <summary>
        /// 奖品说明
        /// </summary>
        public string Summary { set; get; }

        /// <summary>
        /// 奖品所需积分
        /// </summary>
        public int Integral { get; set; }
    }

	/// <summary>
	/// 奖励查询
	/// </summary>
	public class TrainingOrganizationPrizeQuery:BaseQuery<TrainingOrganizationPrize>
	{

	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationPrizeRepository : QueryRepositoryBase<TrainingOrganizationPrize, TrainingOrganizationPrizeQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationPrizeRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
