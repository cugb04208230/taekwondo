using System.Collections.Generic;
using Common.Data;

namespace Taekwondo.Data.Entities
{
    public class Menu:Entity
    {
		/// <summary>
		/// 标签名
		/// </summary>
	    public string Label { get; set; }

		/// <summary>
		/// 地址
		/// </summary>
		public string Url { set; get; }

	    /// <summary>
	    /// 图标地址
	    /// </summary>
	    public string IconUrl { set; get; }

		/// <summary>
		/// 父标签
		/// </summary>
		public string ParentId { get; set; }

		/// <summary>
		/// 子标签
		/// </summary>
		public List<Menu> Children { set; get; }

	}
}
