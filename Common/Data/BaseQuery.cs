using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Common.Data
{

    /// <summary>
    /// represents query conditions object against a entity
    /// </summary>
    [Serializable]
    public class BaseQuery
    {
        private int? _pageIndex;
        private int? _pageSize;
         public BaseQuery()
        {
            Includes = new string[0];
            DirectionList = new KeyValuePair<string, OrderDirection>[0];
        }
        ///// <summary>
        ///// 登陆凭证
        ///// </summary>
        //public string Token { get; set; }
        public long? Id { get; set; }
        /// <summary>
        /// represents query if contains BaseEntity.Id
        /// </summary>
        public long[] Ids { get; set; }
        /// <summary>
        /// 创建时间范围
        /// </summary>
        public DateTime? CreatedAtFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatedAtTo { get; set; }
        /// <summary>
        /// 更新时间范围
        /// </summary>
        public DateTime? LastModifiedAtFrom { get; set; }
        /// <summary>
        /// 更新时间范围
        /// </summary>
        public DateTime? LastModifiedAtTo { get; set; }

        /// <summary>
        /// Id范围
        /// </summary>
        public long? IdFrom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? IdTo { get; set; }

        public int? PageIndex
        {
            get => _pageIndex;
            set => _pageIndex=value ?? 1;
        }

        public int? PageSize
        {
            get => _pageSize;
            set => _pageSize = value ?? 10;
        }

        public int Index => PageIndex ?? 1;

        /// <summary>
        /// take number
        /// </summary>
        public int Take => PageSize??10;

        /// <summary>
        /// skip number
        /// </summary>
        public int? Skip => (PageIndex - 1) * PageSize;

        public List<OrderField> OrderBys { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string[] Includes { get; set; }

        /// <summary>
        /// 方向列表
        /// </summary>
        public KeyValuePair<string, OrderDirection>[] DirectionList { get; set; }
        
    }

    public class OrderField
    {
	    public OrderField(string orderBy, OrderDirection orderDirection)
	    {
		    OrderBy = orderBy;
		    OrderDirection = orderDirection;
		}
	    public OrderField()
	    {
	    }

		/// <summary>
		/// order field
		/// </summary>
		public string OrderBy { get; set; }
        /// <summary>
        /// order direction
        /// </summary>
        public OrderDirection? OrderDirection { get; set; }
    }

    [Serializable]
    public class BaseQuery<TEntity> : BaseQuery 
    {
        public Type EntityType => typeof(TEntity);
    }

    /// <summary>
    /// order direction
    /// </summary>
    public enum OrderDirection
    {
        [Description("升序")]
        Asc = 0,
        [Description("降序")]
        Desc = 1,
    }
}
