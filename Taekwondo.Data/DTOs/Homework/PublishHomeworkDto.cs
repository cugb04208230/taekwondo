using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <inheritdoc />
	/// <summary>
	/// 发布家庭作业
	/// </summary>
    public class PublishHomeworkRequest:BaseRequest<PublishHomeworkResponse>
    {
		/// <summary>
		/// 发布家庭作业Id
		/// </summary>
	    public long ClassId { get; set; }

	    /// <summary>
	    /// 作业标题
	    /// </summary>
	    public string Title { set; get; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Summary { set; get; }

	    /// <summary>
	    /// 文件列表(文件名称，以逗号隔开)
	    /// </summary>
	    public string Files { set; get; }
	    /// <summary>
	    /// 封面图片
	    /// </summary>
	    public string Images { set; get; }
	}
	/// <summary>
	/// 发布家庭作业结果
	/// </summary>
	public class PublishHomeworkResponse
	{
	}
}
