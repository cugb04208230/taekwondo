using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Genearch
{
	/// <inheritdoc />
	/// <summary>
	/// 家长新增
	/// </summary>
    public class GenearchAddRequest:BaseRequest<GenearchAddResponse>
    {
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
        /// 密码
        /// </summary>
        public string Password { set; get; }



        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { set; get; }
    }
	/// <summary>
	/// 家长新增结果
	/// </summary>
	public class GenearchAddResponse
	{
	}

    /// <inheritdoc />
    /// <summary>
    /// 家长更新
    /// </summary>
    public class GenearchUpdateRequest : BaseRequest<GenearchUpdateResponse>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { set; get; }

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

    /// <summary>
    /// 家长更新结果
    /// </summary>
    public class GenearchUpdateResponse
    {
    }
}
