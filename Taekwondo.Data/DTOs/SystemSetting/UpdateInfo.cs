using System;
using System.Collections.Generic;
using System.Text;

namespace Taekwondo.Data.DTOs.SystemSetting
{
	/// <summary>
	/// 更新信息获取
	/// </summary>
    public class UpdateInfoRequest: BaseRequest<UpdateInfoResponse>
	{

	}

	/// <summary>
	/// 更新信息结果
	/// </summary>
	public class UpdateInfoResponse
	{

		/// <summary>
		/// Android应用版本号
		/// </summary>
		public string AppEditionAndroid { set; get; }

		/// <summary>
		/// Android是否强制更新
		/// </summary>
		public bool AppAndroidIsMandatoryUpdate { set; get; }

		/// <summary>
		/// 更新地址
		/// </summary>
		public string Path { set; get; }


		/// <summary>
		/// ios应用版本号
		/// </summary>
		public string AppEditionIos { set; get; }

		/// <summary>
		/// ios是否强制更新
		/// </summary>
		public bool AppIosIsMandatoryUpdate { set; get; }
	}
}
