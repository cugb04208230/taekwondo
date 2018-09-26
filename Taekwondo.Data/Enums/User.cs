using System.ComponentModel;

namespace Taekwondo.Data.Enums
{
    /// <summary>
    /// 用户账号类型
    /// </summary>
    [Description("用户账号类型")]
    public enum UserType
    {
		/// <summary>
		/// 培训机构
		/// </summary>
		[Description("培训机构场馆管理员")]
		TrainingOrganizationManager =1,
		/// <summary>
		/// 培训机构老师
		/// </summary>
		[Description("培训机构老师")]
		TrainingOrganizationTeacher =2,
		/// <summary>
		/// 培训机构学员家长
		/// </summary>
		[Description("培训机构学员家长")]
		TrainingOrganizationGenearch =3,

		/// <summary>
		/// 企业管理员
		/// </summary>
	    [Description("企业管理员")]
		EntManager =4
    }

    /// <summary>
    /// 用户账号状态
    /// </summary>
    public enum UserAccountEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=1,
        /// <summary>
        /// 停用
        /// </summary>
        Stop=2,
    }
}
