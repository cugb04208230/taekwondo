using System;
using Newtonsoft.Json;

namespace Taekwondo.Data.DTOs.TrainingOrganization
{
	/// <inheritdoc />
	/// <summary>
	/// 新增校区
	/// </summary>
	public class TrainingOrganizationAddRequest:BaseRequest<TrainingOrganizationAddResponse>
    {
		/// <summary>
		/// 校区名称
		/// </summary>
	    public string Name { get; set; }

		/// <summary>
		/// 校区地址
		/// </summary>
		public string Address { set; get; }
		
    }
	/// <summary>
	/// 新增培训机构结果
	/// </summary>
	public class TrainingOrganizationAddResponse
	{
	}

	/// <summary>
	/// 培训机构
	/// </summary>
	public class TrainingOrganizationDto
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		[JsonProperty("id")]
		public long Id { set; get; }

        /// <summary>
        /// 校区名称
        /// </summary>
        [JsonProperty("name")]
		public string TrainingOrganizationName { set; get; }

        /// <summary>
		/// 校区地址
		/// </summary>
		public string Address { set; get; }

    }


	/// <inheritdoc />
	/// <summary>
	/// 新增校区
	/// </summary>
	public class TrainingOrganizationUpdateRequest : BaseRequest<TrainingOrganizationUpdateResponse>
	{
		public long Id { set; get; }

		/// <summary>
		/// 校区名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 校区地址
		/// </summary>
		public string Address { set; get; }

	}
	/// <summary>
	/// 新增培训机构结果
	/// </summary>
	public class TrainingOrganizationUpdateResponse
	{
	}


	/// <inheritdoc />
	/// <summary>
	/// 删除校区
	/// </summary>
	public class TrainingOrganizationDeleteRequest : BaseRequest<TrainingOrganizationDeleteResponse>
	{
		public long Id { set; get; }

	}
	/// <summary>
	/// 删除校区结果
	/// </summary>
	public class TrainingOrganizationDeleteResponse
	{
	}

}
