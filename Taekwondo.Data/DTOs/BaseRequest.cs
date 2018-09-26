using System.ComponentModel;
using Newtonsoft.Json;

namespace Taekwondo.Data.DTOs
{
	/// <summary>
	/// 基础请求
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class BaseRequest<T> where T : class
	{
		/// <summary>
		/// 请求模型
		/// </summary>
		[DefaultValue(null)]
		[JsonIgnore]
		public T ResponseModel { set; get; }

		/// <summary>
		/// 票据
		/// </summary>
		public string Token { set; get; }
	}
}
