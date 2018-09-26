using System.ComponentModel;

namespace Taekwondo.Data.Enums
{
	public enum TimeOfDayType
	{
		[Description("上午")]
		Morning =1,
		[Description("下午")]
		Afternoon =2,
		[Description("傍晚")]
		Evening =3
	}
}
