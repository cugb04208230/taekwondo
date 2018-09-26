using Common.Data;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 用户账号信息
    /// </summary>
    public class UserAccount:Entity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 用户类型
        /// <see cref="Enums.UserType"/>
        /// </summary>
        public int UserType { set; get; }

        /// <summary>
        /// 账号状态
        /// <see cref="Enums.UserAccountEnum"/>
        /// </summary>
        public int Status { set; get; }

		/// <summary>
		/// 用户名称
		/// </summary>
	    public string UserName { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
	    public string HeadPic { get; set; }
    }

	/// <summary>
	/// 账户查询
	/// </summary>
	public class UserAccountQuery : BaseQuery<UserAccount>
	{
	}


	/// <inheritdoc />
	public class UserAccountRepository : QueryRepositoryBase<UserAccount, UserAccountQuery>
	{
		/// <inheritdoc />
		public UserAccountRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

	}
}
