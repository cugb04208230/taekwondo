using Common.Data;
using Common.Util;
using Newtonsoft.Json;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Class
{
	/// <inheritdoc />
	/// <summary>
	/// 班级查询
	/// </summary>
    public class TrainingOrganizationClassQueryRequest:BasePageRequest<TrainingOrganizationClassQueryResponse>
    {
		/// <summary>
		/// 班级名称，支持模糊查询
		/// </summary>
	    public string Name { get; set; }

        /// <summary>
		/// 校区ID
		/// </summary>
	    public long? TrainingOrganizationId { get; set; }
        /// <summary>
        /// 学员Id(家长查询时必填)
        /// </summary>
        public long? StudentId { set; get; }
	}
	/// <inheritdoc />
	/// <summary>
	/// 管理员班级查询
	/// </summary>
	public class AdminTrainingOrganizationClassQueryRequest : BasePageRequest<TrainingOrganizationClassQueryResponse>
	{
		/// <summary>
		/// 班级名称，支持模糊查询
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 校区ID
		/// </summary>
		public long? TrainingOrganizationId { get; set; }
	}
	/// <inheritdoc />
	/// <summary>
	/// 教师班级查询
	/// </summary>
	public class TeacherTrainingOrganizationClassQueryRequest : BasePageRequest<TrainingOrganizationClassQueryResponse>
	{
		/// <summary>
		/// 班级名称，支持模糊查询
		/// </summary>
		public string Name { get; set; }

	}
	/// <inheritdoc />
	/// <summary>
	/// 家长班级查询
	/// </summary>
	public class GenearchTrainingOrganizationClassQueryRequest : BasePageRequest<TrainingOrganizationClassQueryResponse>
	{
		/// <summary>
		/// 学员Id(非必填)
		/// </summary>
		public long? StudentId { set; get; }
	}

	/// <summary>
	/// 班级查询结果
	/// </summary>
	public class TrainingOrganizationClassQueryResponse:QueryResult<TrainingOrganizationClass>
	{
	}

	/// <summary>
	/// 班级模型
	/// </summary>
	public class ClassDto
	{
		/// <summary>
		/// 班级标识
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 培训机构班级名称
		/// </summary>
		[JsonProperty("name")]
		public string ClassName { set; get; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }

		/// <summary>
		/// 校区名称
		/// </summary>
		public string OrgName { set; get; }

		/// <summary>
		/// 段位描述
		/// </summary>
		public string DanDesc => Dan.GetDescription();
	}
}
