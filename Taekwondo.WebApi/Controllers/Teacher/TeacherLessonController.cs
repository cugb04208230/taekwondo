using System.Linq;
using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Lesson;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers.Teacher
{
	/// <inheritdoc />
	/// <summary>
	/// 课程
	/// </summary>
	[Auth]
	[Route("api/teacher/lesson/[action]")]
	public class TeacherLessonController : BaseController
	{

		private readonly TrainingOrganizationClassLessonService _lessonService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="lessonService"></param>
		public TeacherLessonController(TrainingOrganizationClassLessonService lessonService)
		{
			_lessonService = lessonService;
		}

		/// <summary>
		/// 获取课程列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult LessonList(TeacherClassLessonListRequest request)
		{
			var response = _lessonService.TeacherLessonList(request);
			return this.Success(response);
		}

		/// <summary>
		/// 获取课程签到列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult LessonSignList(TeacheClassLessonSignListRequest request)
		{
			var response = _lessonService.TeacherClassLessonSignList(request);
			return this.Success(response.List);
		}

		/// <summary>
		/// 获取审批列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult ApprovalList(TeacherClassApprovalListRequest request)
		{
			var response = _lessonService.TeacherClassApprovalList(request);
			if (response.List != null && response.List.Any())
			{
				foreach (var teacherClassApprovalListItemDto in response.List)
				{
					teacherClassApprovalListItemDto.HeadPic = GetHeadPic(teacherClassApprovalListItemDto.StudentId);
				}
			}
			return this.Success(response.List);
		}

		/// <summary>
		/// 审批请求（请假/补课）
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Approval(TeacherClassApprovalRequest request)
		{
			_lessonService.TeacherClassApproval(request);
			return this.Success();
		}

		/// <summary>
		/// 老师批量签到
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult LessonSign(TeacherLessonSignRequest request)
		{
			_lessonService.TeacherLessonSign(request);
			return this.Success();
		}


	}
}
