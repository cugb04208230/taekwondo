using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Taekwondo.Data.DTOs
{

	/// <inheritdoc />
	/// <summary>
	/// DTO数据属性
	/// </summary>
	public class HttpDtoAttribute : Attribute
	{
		/// <summary>
		/// 请求路径
		/// </summary>
		public string Path { set; get; }

		/// <summary>
		/// 请求方法
		/// </summary>
		public string Method { set; get; }


	}
}
