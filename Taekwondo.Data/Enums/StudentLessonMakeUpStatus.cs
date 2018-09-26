using System.ComponentModel;

namespace Taekwondo.Data.Enums
{
    public enum StudentLessonMakeUpStatus
    {
		[Description("待审批")]
	    Applied=1,
		[Description("已通过")]
	    ApprovalSuccess=2,
		[Description("已拒绝")]
	    ApprovalFail=3,
	    [Description("未申请")]
	    NotSubmit = 4
	}

	public enum StudentLessonLeaveStatus
	{
		[Description("待审批")]
		Submit =1,
		[Description("已通过")]
		Agreed =2,
		[Description("已拒绝")]
		Refused =3,
		[Description("未申请")]
		NotSubmit = 4
	}
}
