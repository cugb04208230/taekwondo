using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Common.Data
{
    [Serializable]
    public class QueryResult<T>
    {
        [JsonIgnore]
        public BaseQuery<T> Query { set; get; }

        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; }


        /// <summary>
        /// 总页数
        /// </summary>
        public int? PageCount {
            get  {
                if (Query==null||Query.PageSize==null)
                {
                    return Total / 10 + ((Total % 10) > 0 ? 1 : 0);
                }
                return Total / Query.PageSize + (Total % Query.PageSize) > 0 ? 1 : 0;

            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int? PageIndex
        {
            get
            {
                if (Query == null)
                    return 1;
                return Query.Index;

            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int? PageSize
        {
            get
            {
                if (Query == null)
                    return 10;
                return Query.Take;

            }
        }


        //        /// <summary>
        //        /// 当前页
        //        /// </summary>
        //        public int CurrentPage => Query.Index;
        //
        //		/// <summary>
        //		/// 每一页尺寸
        //		/// </summary>
        //        public int PrePage => Query.Take;
        //
        //		/// <summary>
        //		/// 最后一页页码
        //		/// </summary>
        //        public int LastPage => Total / Query.Take + (Total % Query.Take == 0 ? 0 : 1);
        //
        //		/// <summary>
        //		/// 下一页地址
        //		/// </summary>
        //        public int NextPageUrl { set; get; }
        //
        //		/// <summary>
        //		/// 上一页地址
        //		/// </summary>
        //        public int PrevPageUrl { set; get; }
        //
        //		/// <summary>
        //		/// 当前页 数据起始序号
        //		/// </summary>
        //        public int From => (Query.Index - 1) * List.Count() + 1;
        //
        //		/// <summary>
        //		/// 当前页 数据结束序号
        //		/// </summary>
        //        public int To => From + List.Count() - 1;
    }
}
