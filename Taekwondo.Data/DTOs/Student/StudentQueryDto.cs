using System.Collections.Generic;
using System.ComponentModel;
using Common.Data;
using Common.Util;
using Newtonsoft.Json;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Student
{
	/// <inheritdoc />
	/// <summary>
	/// 学员
	/// </summary>
	[HttpDto(Path="/api/",Method = "Get")]
    public class StudentQueryRequest: BasePageRequest<StudentQueryResponse>
	{
		/// <summary>
		/// 当查询角色为教师或者场馆管理人员时选填
		/// </summary>
		[Description("班级,当查询角色为教师或者场馆管理人员时选填")]
		public long? ClassId { get; set; }

	 
	}


    [HttpDto(Path = "/api", Method = "Get")]
    public class StudentQueryOneRequest : BaseRequest<StudentQueryOneResponse>
    {
        [Description("查单个学员 学员的ID")]
        public long StudentId { get; set; }  
    }

    
    /// <summary>
    /// 学员查询结果
    /// </summary>
    public class StudentQueryOneResponse : GenearchChild
    {
		public List<KeyValuePair<string,string>> Parents { set; get; }
    }
    /// <summary>
    /// 学员查询结果
    /// </summary>
    public class StudentQueryResponse: QueryResult<GenearchChild>
	{
	}

	/// <summary>
	/// 学员模型
	/// </summary>
	public class StudentDto
	{
		/// <summary>
		/// 标识
		/// </summary>
		public virtual long Id { get; set; }

		/// <summary>
		/// 称谓
		/// </summary>
		public string Appellation { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		[JsonProperty("name")]
		public string GenearchChildName { get; set; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }

        /// <summary>
        /// 段位描述
        /// </summary>
        public string DanDesc => Dan.GetDescription();


		/// <summary>
		/// 班级名称
		/// </summary>
		public string ClassName { set; get; }

		/// <summary>
		/// 校区名称
		/// </summary>
		public string TrainingOrganizationName { set; get; }
	}
}
