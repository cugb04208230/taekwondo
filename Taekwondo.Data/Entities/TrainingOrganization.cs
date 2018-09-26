using System;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;
using Newtonsoft.Json;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构
    /// </summary>
    public class TrainingOrganization:Entity
    {
		/// <summary>
		/// 场馆所属企业Id
		/// </summary>
		public long TrainingOrganizationEntId { set; get; }

		/// <summary>
		/// 场馆名称
		/// </summary>
		[JsonProperty("name")]
		public string TrainingOrganizationName { set; get; }

        /// <summary>
        /// 场馆简介
        /// </summary>
        public string Summary { set; get; }

		/// <summary>
		/// 营业天数
		/// </summary>
		[JsonIgnore]
		public string OpeningDay { set; get; }

		/// <summary>
		/// 营业时间开始
		/// </summary>
		[JsonIgnore]
		public DateTime? BusinessHoursFromAt { set; get; }

		/// <summary>
		/// 营业时间结束
		/// </summary>
		[JsonIgnore]
		public DateTime? BusinessHoursToAt { set; get; }

		/// <summary>
		/// 场馆图片
		/// </summary>
		[JsonIgnore]
		public string Pictures { set; get; }

		/// <summary>
		/// 场馆管理员Id
		/// </summary>
		[JsonIgnore]
		public long TrainingOrganizationManagerUserId { set; get; }

		/// <summary>
		/// 校区地址
		/// </summary>
	    public string Address { get; set; }
    }

	/// <summary>
	/// 培训机构查询条件
	/// </summary>
	public class TrainingOrganizationQuery : BaseQuery<TrainingOrganization>
	{
		/// <summary>
		/// 培训机构超级管理员Id
		/// </summary>
		public long? TrainingOrganizationManagerUserId { set; get; }
		/// <summary>
		/// 场馆名称
		/// </summary>
		public string Name { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationRepository : QueryRepositoryBase<TrainingOrganization, TrainingOrganizationQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		protected override Expression<Func<TrainingOrganization, bool>> Where(TrainingOrganizationQuery query)
		{
			var ex =  base.Where(query);
			if (query.TrainingOrganizationManagerUserId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationManagerUserId == query.TrainingOrganizationManagerUserId);
			}
			if (query.Name.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.TrainingOrganizationName.Contains(query.Name));
			}
			return ex;
		}
	}
}
