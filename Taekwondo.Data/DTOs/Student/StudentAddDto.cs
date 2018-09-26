using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Student
{
	/// <inheritdoc />
	/// <summary>
	/// 学员新增
	/// </summary>
    public class StudentAddRequest:BaseRequest<StudentAddResponse>
    {
		/// <summary>
		/// 家长的手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 称谓
		/// </summary>
	    public string Appellation { get; set; }

		/// <summary>
		/// 学员姓名
		/// </summary>
	    public string Name { get; set; }

		/// <summary>
		/// 学员身份证标识
		/// </summary>
		public string IdCardNo { set; get; }
	    /// <summary>
	    /// 场馆Id
	    /// </summary>
	    public long TrainingOrganizationId { set; get; }

		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }

        /// <summary>
		/// 剩余课时
		/// </summary>
        public long LessionRemain { set; get; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }
    }
	/// <summary>
	/// 学员新增结果
	/// </summary>
	public class StudentAddResponse
	{
	}

    /// <inheritdoc />
    /// <summary>
    /// 学员更新
    /// </summary>
    public class StudentUpdateRequest : BaseRequest<StudentUpdateResponse>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }

//        /// <summary>
//        /// 家长Id
//        /// </summary>
//        public long GenearchId { get; set; }


		/// <summary>
		/// 家长手机号
		/// </summary>
		public string Mobile { set; get; }
		
        /// <summary>
        /// 称谓
        /// </summary>
        public string Appellation { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }

        /// <summary>
        /// 段位
        /// </summary>
        public DanType Dan { set; get; }

        /// <summary>
		/// 剩余课时
		/// </summary>
        public long LessionRemain { set; get; }
    }

    /// <summary>
    /// 学员更新结果
    /// </summary>
    public class StudentUpdateResponse
    {
    }
}
