using System.Collections.Generic;

namespace Taekwondo.Data.DTOs.Account
{
	/// <summary>
	/// 用户信息
	/// </summary>
    public class UserInfo
    {
		/// <summary>
		/// 用户编号
		/// </summary>
	    public long UserId { get; set; }
		/// <summary>
		/// 用户类型
		///1.培训机构
		///2.培训机构老师
		///3.培训机构学员
		/// </summary>
		public int Type { set; get; }

		/// <summary>
		/// 头像
		/// </summary>
		public string HeadPic { set; get; }

		/// <summary>
		/// 用户名
		/// </summary>
	    public string UserName { get; set; }
		
		/// <summary>
		/// 登录返回关联数据
		/// 老师：班级Id,班级名称
		/// 家长：学生Id,学生名称,称谓
		/// </summary>
	    public List<object> ExtendData  { get; set; }
    }
}
