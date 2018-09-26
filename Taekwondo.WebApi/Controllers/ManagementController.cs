using System.Linq;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Student;
using Taekwondo.Data.DTOs.Teacher;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;
using Taekwondo.Service;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 后台管理
	/// </summary>
	[Route("mgr/[action]")]
    public class ManagementController:BaseController
	{
		private readonly AccountService _accountService;
		private readonly TrainingOrganizationTeacherService _trainingOrganizationTeacherService;
		private readonly GenearchChildService _genearchChildService;

		/// <summary>
		/// ctor.
		/// </summary>
		public ManagementController(AccountService accountService, TrainingOrganizationTeacherService trainingOrganizationTeacherService, GenearchChildService genearchChildService)
		{
			_accountService = accountService;
			_trainingOrganizationTeacherService = trainingOrganizationTeacherService;
			_genearchChildService = genearchChildService;
		}


		/// <summary>
		/// 主页
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 老师主页
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TeacherIndex(TeacherQueryRequest request)
		{
			var query = ModelMapUtil.AutoMap(request, new TrainingOrganizationTeacherQuery());
			var queryResult = _trainingOrganizationTeacherService.QueryTeacher(query);
			PageRender(queryResult);
			ViewBag.List = queryResult.List.ToList();
			return View();
		}
		/// <summary>
		/// 老师新增
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TeacherAdd(TeacherQueryRequest request)
		{
			var query = ModelMapUtil.AutoMap(request, new TrainingOrganizationTeacherQuery());
			var queryResult = _trainingOrganizationTeacherService.QueryTeacher(query);
			PageRender(queryResult);
			ViewBag.List = queryResult.List.ToList();
			return View();
		}

		/// <summary>
		/// 学生主页
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult StudentIndex(StudentQueryRequest request)
		{
			var query = ModelMapUtil.AutoMap(request,new GenearchChildQuery());
			var queryResult = _genearchChildService.Query(query);
			PageRender(queryResult);
			ViewBag.List = queryResult.List.ToList();
			return View();
		}
	}
}
