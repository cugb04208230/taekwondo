using System;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;
using Newtonsoft.Json;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 家长
    /// </summary>
    public class Genearch:Entity
    {

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
        public string GenearchName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }

        /// <summary>
        /// 性别
        /// <see cref="Common.Enums.Gender"/>
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { set; get; }
    }

	/// <summary>
	/// 家长信息查询
	/// </summary>
	public class GenearchQuery:BaseQuery<Genearch>
	{
		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

        /// <summary>
		/// 班级
		/// </summary>
		public long? ClassId { get; set; }

        public long TrainingOrganizationId { get; set; }
    }

	/// <inheritdoc />
	/// <summary>
	/// 家长
	/// </summary>
	public class GenearchRepository:QueryRepositoryBase<Genearch, GenearchQuery>
	{
		/// <inheritdoc />
		public GenearchRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		protected override Expression<Func<Genearch, bool>> Where(GenearchQuery query)
		{
			var ex= base.Where(query);
			if (query.Name.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.GenearchName.Contains(query.Name));
			}
			if (query.Mobile.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.GenearchName.Contains(query.Mobile));
			}
			return ex;
		}
	}
}
