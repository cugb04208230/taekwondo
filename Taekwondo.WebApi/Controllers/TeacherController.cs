using System.Linq;
using Common.Data;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Teacher;
using Taekwondo.Data.Entities;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 教师管理(场馆管理后台使用)
	/// </summary>
	[Auth]
	[Route("api/[controller]/[action]")]
	public class TeacherController:BaseController
	{
		private readonly TrainingOrganizationTeacherService _trainingOrganizationTeacherService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="trainingOrganizationTeacherService"></param>
		public TeacherController(TrainingOrganizationTeacherService trainingOrganizationTeacherService)
	    {
			_trainingOrganizationTeacherService= trainingOrganizationTeacherService;
		}

		/// <summary>
		/// 查询教师列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Query(TeacherQueryRequest request)
		{
			var query = new TrainingOrganizationTeacherQuery();
            query.PageIndex = request.PageIndex;
            query.PageSize = request.PageSize;
			query.TrainingOrganizationManageUserId = UserAccountId;
			var queryResult = _trainingOrganizationTeacherService.QueryTeacher(query);
			var response = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new TeacherDto())).ToList();
			return this.Success(response);
		}

		/// <summary>
		/// 新增教师
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Add(TeacherAddRequest request)
		{
			_trainingOrganizationTeacherService.AddTeacher(UserAccountId??0,request.Mobile,request.Password,request.Name);
			return this.Success();
		}

		/// <summary>
		/// 修改教师
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Update(TeacherUpdateRequest request)
		{
			_trainingOrganizationTeacherService.UpdateTeacher(UserAccountId ?? 0, request);
			return this.Success();
		}


        /// <summary>
		/// 删除一个教师
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public IActionResult Delete(TeacherDeleteRequest request)
        {
            var user = UserAccount;
            _trainingOrganizationTeacherService.DeleteTeacher(UserAccountId ?? 0, request.Id);
            return this.Success();
        }
    }
}
