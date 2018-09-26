using Common.Data;
using Newtonsoft.Json;
using Taekwondo.Data.Entities;

namespace Taekwondo.Data.DTOs.Teacher
{
	/// <inheritdoc />
	/// <summary>
	/// 教师查询
	/// </summary>
    public class TeacherQueryRequest:BasePageRequest<TeacherQueryResponse>
    {
		/// <summary>
		/// 校区Id
		/// </summary>
		public long? EntId { set; get; }

		/// <summary>
		/// 老师名称，支持模糊查询
		/// </summary>
	    public string Name { get; set; }
	    /// <summary>
	    /// 老师手机号，支持模糊查询
	    /// </summary>
	    public string Mobile { get; set; }

	    /// <summary>
	    /// 老师手机号或者名称
	    /// </summary>
	    public string CommonText { set; get; }

	}
	/// <summary>
	/// 教师查询结果
	/// </summary>
	public class TeacherQueryResponse :QueryResult<TrainingOrganizationTeacher>
	{
	}

	/// <summary>
	/// 教师
	/// </summary>
	public class TeacherDto
	{
		/// <summary>
		/// 姓名
		/// </summary>
		[JsonProperty("name")]
		public string TeacherName { set; get; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public long TeacherId { set; get; }

        /// <summary>
		/// Id
		/// </summary>
		public long Id { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }

		/// <summary>
		/// 头像
		/// </summary>
		public string HeadPic { set; get; }
	}
}
