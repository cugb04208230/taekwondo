namespace Taekwondo.Data.DTOs.Teacher
{
    /// <inheritdoc />
    /// <summary>
    /// 教师添加结果
    /// </summary>
    public class TeacherAddRequest : BaseRequest<TeacherAddResponse>
    {
        /// <summary>
        /// 俱乐部管理员id
        /// </summary>
        public long ManagerId { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TeacherAddResponse
    {
    }


    /// <inheritdoc />
    /// <summary>
    /// 教师更新结果
    /// </summary>
    public class TeacherUpdateRequest : BaseRequest<TeacherUpdateResponse>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 场馆id
        /// </summary>
        public long TrainingOrganizationId { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TeacherUpdateResponse
    {
    }

    public class TeacherDeleteRequest : BaseRequest<TeacherDeleteResponse>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { set; get; }
    }

    public class TeacherDeleteResponse
    {
    }
}
