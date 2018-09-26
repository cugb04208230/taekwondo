using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Lesson;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers.Genearch
{
	/// <inheritdoc />
	/// <summary>
	/// 课程
	/// </summary>
	[Auth]
	[Route("api/student/lesson/[action]")]
	public class StudentLessonController:BaseController
	{
		private readonly TrainingOrganizationClassLessonService _lessonService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="lessonService"></param>
		public StudentLessonController(TrainingOrganizationClassLessonService lessonService)
		{
			_lessonService = lessonService;
		}

	    /// <summary>
	    /// 学员课程签到
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
	    public IActionResult LessonSign(StudentLessonSignRequest request)
		{
			_lessonService.StudentLessonSign(request);
			return this.Success();
	    }

	    /// <summary>
	    /// 学员请假
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
	    public IActionResult LessonLeave(StudentLessonLeaveRequest request)
	    {
		    _lessonService.StudentLessonLeave(request);
			return this.Success(request.ResponseModel);
	    }

	    /// <summary>
	    /// 学员补课申请
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
	    public IActionResult LessonMakeUp(StudentLessonMakeUpRequest request)
	    {
		    _lessonService.StudentLessonMakeUp(request);
			return this.Success(request.ResponseModel);
	    }

	    /// <summary>
	    /// 学员课程列表
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
		public IActionResult LessonList(StudentLessonListRequest request)
	    {
		    request.ResponseModel = _lessonService.StudentLessonList(request);
		    return this.Success(request.ResponseModel);
	    }

		/// <summary>
		/// 学员获取可补课列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult ReserveLessonList(StudentLessonReserveListRequest request)
		{
			request.ResponseModel = _lessonService.StudentLessonReserveList(request);
			return this.Success(request.ResponseModel);
		}

	}

}
