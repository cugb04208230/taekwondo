using Common.Data;

namespace Taekwondo.Data.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 系统配置
	/// </summary>
    public class SystemSetting:Entity
    {
		/// <summary>
		/// 编码
		/// </summary>
	    public string Code { get; set; }

		/// <summary>
		///值
		/// </summary>
		public string Value { set; get; }

    }

	/// <summary>
	/// 系统配置编码
	/// </summary>
	public static class SystemSettingCode
	{
		/// <summary>
		/// android应用版本号
		/// </summary>
		public static string AppEditionAndroid = "AppEditionAndroid";
		/// <summary>
		/// ios应用版本号
		/// </summary>
		public static string AppEditionIos = "AppEditionIos";

		/// <summary>
		/// android是否强制更新
		/// </summary>
		public static string AppAndroidIsMandatoryUpdate = "AppAndroidIsMandatoryUpdate";
		/// <summary>
		/// ios是否强制更新
		/// </summary>
		public static string AppIosIsMandatoryUpdate = "AppIosIsMandatoryUpdate";
	}
}
