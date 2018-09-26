using System;
using System.Collections.Generic;
using System.Text;
using Common.Data;
using Common.Util;
using Newtonsoft.Json;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.DTOs.Lesson
{

	#region TrainingOrganizationManager

	#region LessonAddDto

	/// <inheritdoc />
	/// <summary>
	/// 新增班级课程请求
	/// </summary>
	public class LessonAddRequest : BaseRequest<LessonAddResponse>
	{
		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { get; set; }

		/// <summary>
		/// 课程开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 课程结束时间
		/// </summary>
		public DateTime EndTime { set; get; }
	}

	/// <summary>
	/// 新增班级课程结果
	/// </summary>
	public class LessonAddResponse
	{
	}

	#endregion

	#region LessonReserveDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程预定请求
	/// </summary>
	public class LessonReserveRequest : BaseRequest<LessonReserveResponse>
	{
		/// <summary>
		/// 学员编号
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 课程编号
		/// </summary>
		public long[] LessonIds { set; get; }
	}

	/// <summary>
	/// 学员课程预定结果
	/// </summary>
	public class LessonReserveResponse
	{
	}

	#endregion

	#endregion

	#region StudentLessonSignDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程签到请求
	/// </summary>
	public class StudentLessonSignRequest : BaseRequest<StudentLessonSignResponse>
	{
		/// <summary>
		/// 学员编号
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 课程编号
		/// </summary>
		public long Id { set; get; }
	}

	/// <summary>
	/// 学员课程签到结果
	/// </summary>
	public class StudentLessonSignResponse
	{
	}

	#endregion

	#region StudentLessonLeaveDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程请假请求
	/// </summary>
	public class StudentLessonLeaveRequest : BaseRequest<StudentLessonLeaveResponse>
	{
		/// <summary>
		/// 学员编号
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 课程编号
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { set; get; }
	}

	/// <summary>
	/// 学员课程请假结果
	/// </summary>
	public class StudentLessonLeaveResponse
	{
	}

	#endregion

	#region StudentLessonMakeUpDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程请假补课请求
	/// </summary>
	public class StudentLessonMakeUpRequest : BaseRequest<StudentLessonMakeUpResponse>
	{
		/// <summary>
		/// 原始课程Id
		/// </summary>
		public long OriginalLessonId { set; get; }

		/// <summary>
		/// 学员编号
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 课程编号
		/// </summary>
		public long LessonId { set; get; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { set; get; }
	}

	/// <summary>
	/// 学员课程请假补课请求
	/// </summary>
	public class StudentLessonMakeUpResponse
	{
	}

	#endregion

	#region StudentLessonDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程列表
	/// </summary>
	public class StudentLessonListRequest : BasePageRequest<StudentLessonListResponse>
	{
		/// <summary>
		/// 学员Id
		/// </summary>
		public long StudentId { set; get; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class StudentLessonListResponse : QueryResult<StudentLessonListItemDto>
	{
		/// <summary>
		/// 是否需要续费提醒
		/// </summary>
		public bool NeedNotice => Total <= 10;

		/// <summary>
		/// 之前的十条
		/// </summary>
		public List<StudentLessonListItemDto> BeforeList { set; get; }

		/// <summary>
		/// 剩余课时
		/// </summary>
		public long LessionRemain { set; get; }
	}

	public class StudentLessonListItemDto
	{
		/// <summary>
		/// 课程Id
		/// <see cref="TrainingOrganizationClassStudentLessonMap"/>
		/// </summary>
		public long Id { set; get; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { set; get; }

		/// <summary>
		/// 周几
		/// </summary>
		public string WeekDesc =>
			System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek);

		/// <summary>
		/// 日期
		/// </summary>
		public string Day => StartTime.ToString("MM月dd日");

		/// <summary>
		/// 是否签到
		/// </summary>
		public bool IsSign { set; get; }

		/// <summary>
		/// 是否请假
		/// </summary>
		public bool IsLeave { set; get; }

		/// <summary>
		/// 是否为补课
		/// </summary>
		public bool IsMakeUp { set; get; }

		/// <summary>
		/// 是否已经申请补课
		/// </summary>
		public bool HasMakeUp { set; get; }

		/// <summary>
		/// 是否旷课
		/// </summary>
		public bool IsCuttingSchool => !(IsLeave || IsSign || StartTime > DateTime.Now.AddMinutes(-15));

		public StudentLessonLeaveStatus LeaveStatus { set; get; }

		public string LeaveStatusDesc => LeaveStatus.GetDescription();

		public StudentLessonMakeUpStatus MakeUpStatus { set; get; }

		public string MakeUpStatusDesc => MakeUpStatus.GetDescription();
	}

	#endregion

	#region StudentLessonReserve

	public class StudentLessonReserveListRequest : BasePageRequest<StudentLessonReserveListResponse>
	{
		/// <summary>
		/// 学员Id
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 上午/下午/傍晚
		/// </summary>
		public TimeOfDayType? TimeOfDayType { set; get; }

		/// <summary>
		/// 星期
		/// </summary>
		public DayOfWeek? DayOfWeek { set; get; }
	}

	public class StudentLessonReserveListResponse : QueryResult<StudentLessonReserveListItemDto>
	{
	}

	public class StudentLessonReserveListItemDto
	{

		/// <summary>
		/// 课程Id
		/// </summary>
		public long Id { set; get; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { set; get; }

		/// <summary>
		/// 周几
		/// </summary>
		public string WeekDesc =>
			System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek);

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }

		public string DanDesc => Dan.GetDescription();

		/// <summary>
		/// 是否已经预约
		/// </summary>
		[JsonIgnore]
		public bool HasReserved { set; get; }

		/// <summary>
		/// 班级名称
		/// </summary>
		public string ClassName { set; get; }
	}

	#endregion

	#region TeacherClassLessonList

	public class TeacherClassLessonListRequest : BasePageRequest<TeacherClassLessonListResponse>
	{
		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }
	}

	public class TeacherClassLessonListResponse : QueryResult<TeacherClassLessonListItemDto>
	{
	}

	public class TeacherClassLessonListItemDto
	{

		/// <summary>
		/// 课程Id
		/// <see cref="TrainingOrganizationClassLesson"/>
		/// </summary>
		public long Id { set; get; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { set; get; }

		/// <summary>
		/// 周几
		/// </summary>
		public string WeekDesc =>
			System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek);

		/// <summary>
		/// 日期
		/// </summary>
		public string Day => StartTime.ToString("MM月dd日");
	}

	#endregion

	#region TeacherClassLessonSignList

	public class TeacheClassLessonSignListRequest : BasePageRequest<TeacherClassLessonSignListResponse>
	{
		public long LessonId { set; get; }
	}

	public class TeacherClassLessonSignListResponse : QueryResult<TeacherClassLessonSignListItemDto>
	{

	}

	public class TeacherClassLessonSignListItemDto
	{
		/// <summary>
		/// 学员名称
		/// </summary>
		public string StudentName { set; get; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long StudentId { set; get; }

		/// <summary>
		/// 是否已签到
		/// </summary>
		public bool HasSigned { set; get; }

		/// <summary>
		/// 是否已请假
		/// </summary>
		public bool HasLeaved { set; get; }

		/// <summary>
		/// 是否为补课
		/// </summary>
		public bool IsMakeUp { set; get; }
	}


	#endregion

	#region TeacherClassApprovalList

	public class TeacherClassApprovalListRequest : BasePageRequest<TeacherClassApprovalListResponse>
	{
		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }
	}

	public class TeacherClassApprovalListResponse : QueryResult<TeacherClassApprovalListItemDto>
	{
	}

	public class TeacherClassApprovalListItemDto
	{
		/// <summary>
		/// 审核Id
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 学员姓名组合
		/// </summary>
		public string StudentName { set; get; }

		public long StudentId { set; get; }

		public string ClassName { set; get; }

		public string Content
		{
			get
			{
				if (Type == ApprovalType.Leave)
				{
					return
						$"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(TargetStartTime.DayOfWeek)} {TargetStartTime:HH:mm}-{TargetEndTime:HH:mm} 申请请假";
				}
				else
				{
					return
						$"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(TargetStartTime.DayOfWeek)} {TargetStartTime:HH:mm}-{TargetEndTime:HH:mm} 申请补课";
				}
				return string.Empty;
			}
		}

		public string Remark { set; get; }
		[JsonIgnore]
		public DateTime TargetStartTime { set; get; }
		[JsonIgnore]
		public DateTime TargetEndTime { set; get; }

		public string Day => TargetStartTime.ToString("yyyy年MM月dd日");

		/// <summary>
		/// 类型
		/// </summary>
		public ApprovalType Type { set; get; }

		/// <summary>
		/// 类型描述
		/// </summary>
		public string TypeDesc => Type.GetDescription();

		public DanType Dan { set; get; }

		public string DanDesc => Dan.GetDescription();

		public string HeadPic { set; get; }

		/// <summary>
		/// 申请状态
		/// </summary>
		public StudentLessonMakeUpStatus Status { set; get; }

		/// <summary>
		/// 申请状态描述
		/// </summary>
		public string StatusDesc => Status.GetDescription();

	}


	#endregion

	#region TeacherClassApproval

	public class TeacherClassApprovalRequest
	{
		/// <summary>
		/// 标识
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 类型
		/// </summary>
		public ApprovalType Type { set; get; }

		/// <summary>
		/// 是否同意
		/// </summary>
		public bool IsAgree { set; get; }

	}

	#endregion
	
	#region TeacherLessonSignDto
	/// <inheritdoc />
	/// <summary>
	/// 学员课程签到请求
	/// </summary>
	public class TeacherLessonSignRequest : BaseRequest<TeacherLessonSignResponse>
	{
		/// <summary>
		/// 学员编号，逗号隔开
		/// </summary>
		public string StudentIds { set; get; }

		/// <summary>
		/// 课程编号
		/// </summary>
		public long Id { set; get; }
	}

	/// <summary>
	/// 学员课程签到结果
	/// </summary>
	public class TeacherLessonSignResponse
	{
	}

	#endregion

	#region ClassLessonList


	public class ClassLessonListRequest : BasePageRequest<ClassLessonListResponse>
	{
		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassId { set; get; }
	}

	public class ClassLessonListResponse : QueryResult<ClassLessonListItemDto>
	{
	}

	public class ClassLessonListItemDto
	{

		/// <summary>
		/// 课程Id
		/// <see cref="TrainingOrganizationClassLesson"/>
		/// </summary>
		public long Id { set; get; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { set; get; }
		
	}

	#endregion

	#region ClassLessonDelete

	public class ClassLessonDeleteRequest : BaseRequest<ClassLessonDeleteResponse>
	{
		public long Id { set; get; }
	}

	public class ClassLessonDeleteResponse
	{
	}

	#endregion


	#region ClassLessonDelete

	public class ClassLessonUpdateRequest : BaseRequest<ClassLessonUpdateResponse>
	{
		public long Id { set; get; }

		public DateTime Day { set; get; }

		/// <summary>
		/// 开始时间
		/// </summary>
		public string StartTime { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public string EndTime { set; get; }
	}

	public class ClassLessonUpdateResponse
	{
	}

	#endregion
}
