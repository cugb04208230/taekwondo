using System;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 场馆所属企业
	/// </summary>
    public class TrainingOrganizationEnt:Entity
    {
		/// <summary>
		/// 企业名称
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 管理员Id
		/// </summary>
		public long ManagerId { set; get; }
    }

	public class TrainingOrganizationEntQuery : BaseQuery<TrainingOrganizationEnt>
	{
		public string Name { set; get; }

		public long? ManageId { set; get; }


	}

	public class TrainingOrganizationEntRepository : QueryRepositoryBase<TrainingOrganizationEnt, TrainingOrganizationEntQuery>
	{
		public TrainingOrganizationEntRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		protected override Expression<Func<TrainingOrganizationEnt, bool>> Where(TrainingOrganizationEntQuery query)
		{
			var exp = base.Where(query);
			if (query.Name.IsNotNullOrEmpty())
			{
				exp = exp.And(e => e.Name.Contains(query.Name));
			}
			if (query.ManageId.HasValue)
			{
				exp = exp.And(e => e.ManagerId==query.ManageId);
			}
			return exp;
		}
	}
}
