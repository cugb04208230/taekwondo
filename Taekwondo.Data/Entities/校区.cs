using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Taekwondo.Data.Entities
{
    public class 校区
    {
		[Key]
	    public long Id { get; set; }

		public string 名称 { get; set; }

	    public string 地址 { get; set; }

	}

	public class 教师
	{
		[Key]
		public long Id { get; set; }

		public string 名称 { get; set; }

		public string 手机号 { get; set; }
		public string 初始密码 { get; set; }
	}

	public class 班级
	{
		[Key]
		public long Id { get; set; }

		public string 名称 { get; set; }

		public string 所属校区 { get; set; }
		public string 任课老师 { get; set; }
		public string 上课时间段 { get; set; }
		public string 段位 { get; set; }

	}


	public class 家长
	{
		[Key]
		public long Id { get; set; }

		public string 姓名 { get; set; }

		public string 手机号 { get; set; }
		public string 称谓 { get; set; }
		public string 校区 { get; set; }
		public string 班级 { get; set; }
		public string 学员姓名 { get; set; }
		public string 初始密码 { get; set; }

	}

	public class 学员
	{
		[Key]
		public long Id { get; set; }

		public string 姓名 { get; set; }

		public string 校区 { get; set; }
		public string 班级 { get; set; }
		public string 段位 { get; set; }
		public string 剩余课时 { get; set; }

	}
}
