using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Student;
using Taekwondo.Data.Entities;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 学员管理
	/// </summary>
	[Auth]
	[Route("api/[controller]/[action]")]
	public class StudentController:BaseController
	{
		private readonly GenearchChildService _genearchChildService;
        private readonly TrainingOrganizationClassLessonService _trainingOrganizationClassLessonService;
        private readonly TrainingOrganizationClassStudentService _trainingOrganizationClassStudentService;

        /// <summary>
        /// ctor.
        /// </summary>
        public StudentController(TrainingOrganizationClassStudentService trainingOrganizationClassStudentService,GenearchChildService genearchChildService, TrainingOrganizationClassLessonService trainingOrganizationClassLessonService)
		{
			_genearchChildService = genearchChildService;
            _trainingOrganizationClassLessonService = trainingOrganizationClassLessonService;
            _trainingOrganizationClassStudentService = trainingOrganizationClassStudentService;
        }

		/// <summary>
		/// 获取学员列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult Query(StudentQueryRequest request)
		{
			var query = new GenearchChildQuery();
            if (request.ClassId != null)
                query.ClassId = request.ClassId;

            var user = UserAccount;
		  
            query.PageIndex = request.PageIndex;
            query.PageSize = request.PageSize;
			query.TrainingOrganizationManageUserId = user.Id;
			var queryResult = _genearchChildService.Query(query);
           
//            QueryResult<GenearchChild> res = new QueryResult<GenearchChild>();
//
//            //System.Collections.Generic.List<object> list = new System.Collections.Generic.List<object>();
//            List < GenearchChild > li = new List<GenearchChild>();
//            foreach (TrainingOrganizationClassStudent student in queryResult.List)
//            {
//               var child= _genearchChildService.Get(student.GenearchChildId);
//                if (child != null)
//                    li.Add(child);
//            }
//            res.List = li;
//            res.Total = queryResult.Total;

            return this.Success(queryResult);
		}

		/// <summary>
		/// 获取单个学员
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult QueryOne(StudentQueryOneRequest request)
		{

			var studentQueryOneResponse = _genearchChildService.QueryOne(request.StudentId);
			return this.Success(studentQueryOneResponse);
		}

		/// <summary>
        /// 新增学员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult Add(StudentAddRequest request)
		{
			_genearchChildService.AddGenearchChild(UserAccountId??0,request.TrainingOrganizationId,request.Mobile,request.Name, request.Appellation,request.IdCardNo,request.ClassId,request.LessionRemain, request.Dan);
			return this.Success();
		}

        /// <summary>
        /// 删除一个学员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(long id)
        {
            var user = UserAccount;
            _genearchChildService.DeleteChild(user.Id, id);
            return this.Success();
        }

        /// <summary>
		/// 修改一个学员
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
        public IActionResult Update(StudentUpdateRequest request)
        {
            _genearchChildService.UpdateChild(UserAccountId ?? 0, request);
            return this.Success();
        }
    }
}
