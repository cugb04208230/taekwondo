using Common.Data;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 奖品兑换记录
    /// </summary>
    public class TrainingOrganizationPrizeExchangeRecord:Entity
    {
        /// <summary>
        /// 奖品Id
        /// </summary>
        public long TrainingOrganizationPrizeId { get; set; }

        /// <summary>
        /// 兑换用户
        /// </summary>
        public long UserId { set; get; }
    }

	/// <summary>
	/// 奖品兑换查询
	/// </summary>
	public class TrainingOrganizationPrizeExchangeRecordQuery : BaseQuery<TrainingOrganizationPrizeExchangeRecord>
	{
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationPrizeExchangeRecordRepository : QueryRepositoryBase<
		TrainingOrganizationPrizeExchangeRecord, TrainingOrganizationPrizeExchangeRecordQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationPrizeExchangeRecordRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
