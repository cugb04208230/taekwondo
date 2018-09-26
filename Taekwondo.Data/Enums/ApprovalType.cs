using System.ComponentModel;

namespace Taekwondo.Data.Enums
{
    public enum ApprovalType
    {
		[Description("请假")]
		Leave=1,
	    [Description("补课")]
		MakeUp =2
    }
}
