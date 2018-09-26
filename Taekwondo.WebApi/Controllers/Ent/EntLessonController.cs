using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Lesson;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers.Ent
{
	/// <inheritdoc />
	/// <summary>
	/// 课程
	/// </summary>
	[Auth]
	[Route("api/ent/lesson/[action]")]
	public class EntLessonController:BaseController
    {

	    private readonly TrainingOrganizationClassLessonService _lessonService;

	    /// <summary>
	    /// ctor.
	    /// </summary>
	    /// <param name="lessonService"></param>
	    public EntLessonController(TrainingOrganizationClassLessonService lessonService)
	    {
		    _lessonService = lessonService;
	    }

		/// <summary>
		/// 新增班级课程
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
	    public IActionResult LessonAdd(LessonAddRequest request)
	    {
		    _lessonService.LessonAdd(request);
		    return this.Success(new LessonAddResponse());
	    }

	    /// <summary>
	    /// 学员课程预定
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
	    public IActionResult LessonReserve(LessonReserveRequest request)
	    {
		    _lessonService.LessonReserve(request);
		    return this.Success(request.ResponseModel);
	    }

		/// <summary>
		/// 班级课程列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult LessonQuery(ClassLessonListRequest request)
		{
			var response = _lessonService.ClassLessonList(request);
			return this.Success(response);
		}

	    /// <summary>
	    /// 班级课程删除
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
		public IActionResult LessonDelete(ClassLessonDeleteRequest request)
	    {
			_lessonService.ClassDelete(request);
		    return this.Success();
	    }


	    /// <summary>
	    /// 班级课程更新
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    [HttpPost]
		public IActionResult LessonUpdate(ClassLessonUpdateRequest request)
		{
			_lessonService.ClassUpdate(request);
			return this.Success();
	    }
    }
}
