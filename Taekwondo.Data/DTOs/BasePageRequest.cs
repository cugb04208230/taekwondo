namespace Taekwondo.Data.DTOs
{
	/// <inheritdoc />
	/// <summary>
	/// 基础分类请求模型
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class BasePageRequest<T>:BaseRequest<T> where T:class
    {
		/// <summary>
		/// 分页 页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页 每一页数据数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 
		/// </summary>
	    public int Skip => ((PageIndex ?? 1) - 1) * (PageSize ?? 10);
		/// <summary>
		/// 
		/// </summary>
	    public int Take => PageSize ?? 10;
	}
}
