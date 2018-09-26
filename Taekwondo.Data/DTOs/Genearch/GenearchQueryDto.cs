using System.ComponentModel;
using Common.Data;
using Common.Util;
using Newtonsoft.Json;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Genearch
{
	/// <inheritdoc />
	/// <summary>
	/// 家长
	/// </summary>
	[HttpDto(Path="/api/",Method = "Get")]
    public class GenearchQueryRequest: BasePageRequest<GenearchQueryResponse>
	{
		/// <summary>
		/// 当查询角色为教师或者场馆管理人员时选填
		/// </summary>
		[Description("班级,当查询角色为教师或者场馆管理人员时选填")]
		public long? ClassId { get; set; }

		/// <summary>
		/// 当查询角色为教师或者场馆管理人员时选填
		/// </summary>
		[Description("老师,当查询角色为教师或者场馆管理人员时选填")]
		public long? TeacherId { get; set; }

		/// <summary>
		/// 家长手机号或者学员姓名
		/// </summary>
		[Description("家长手机号或者学员姓名")]
		public string CommonText { set; get; }


        public long TrainingOrganizationId { set; get; }
    }

	/// <summary>
	/// 家长查询结果
	/// </summary>
	public class GenearchQueryResponse: QueryResult<GenearchChild>
	{
	}

    /// <summary>
    /// 家长模型
    /// </summary>
    public class GenearchDto
	{
		/// <summary>
		/// 标识
		/// </summary>
		public virtual long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string GenearchName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }

        /// <summary>
        /// 性别
        /// <see cref="Common.Enums.Gender"/>
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public System.DateTime Birthday { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { set; get; }
    }
}
