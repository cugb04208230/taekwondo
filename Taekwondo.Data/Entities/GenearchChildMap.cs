using Common.Data;

namespace Taekwondo.Data.Entities
{
    public class GenearchChildMap:Entity
	{
		/// <summary>
		/// 家长Id
		/// </summary>
		public long GenearchId { get; set; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long GenearchChildId { set; get; }

		/// <summary>
		/// 称谓
		/// </summary>
		public string Appellation { get; set; }
	}

	public class GenearchChildMapQuery:BaseQuery<GenearchChildMap>
	{
	}

	public class GenearchChildMapRepository : QueryRepositoryBase<GenearchChildMap, GenearchChildMapQuery>
	{
		public GenearchChildMapRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}
	}
}
