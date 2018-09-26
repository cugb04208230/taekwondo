using System.Linq;
using Common.Data;
using Common.Models;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Class;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 班级管理，场馆管理使用
	/// </summary>
	[Auth]
	[Route("api/[controller]/[action]")]
	public class ClassController:BaseController
	{
		private readonly TrainingOrganizationClassService _trainingOrganizationClassService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="trainingOrganizationClassService"></param>
		public ClassController(TrainingOrganizationClassService trainingOrganizationClassService)
		{
			_trainingOrganizationClassService = trainingOrganizationClassService;
		}

		/// <summary>
		/// 查询班级
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Query(TrainingOrganizationClassQueryRequest request)
		{
			var user = UserAccount;
			var query = new TrainingOrganizationClassQuery
			{
				Name = request.Name
			};
			if (user.UserType == (int) UserType.TrainingOrganizationManager)
			{
				query.TrainingOrganizationManageUserId = user.Id;
			}
			else if (user.UserType == (int) UserType.TrainingOrganizationTeacher)
			{
				query.TrainingOrganizationTeacherId = user.Id;
			}
            
            else
			{
				query.GenearchChildId = request.StudentId;
			}

            if(request.TrainingOrganizationId!=null)
            {
                query.TrainingOrganizationId = request.TrainingOrganizationId;
            }
          
            query.PageIndex = request.PageIndex;
            query.PageSize = request.PageSize;
            var queryResult = _trainingOrganizationClassService.Query(query);
			QueryResult<ClassDto> res = new QueryResult<ClassDto>();
           
            res.List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new ClassDto())).ToList();
            res.Total = queryResult.Total;
            return this.Success(res);           
		}


		/// <summary>
		/// 管理员查询班级
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult AdminQuery(TrainingOrganizationClassQueryRequest request)
		{
			var user = UserAccount;
			var query = new TrainingOrganizationClassQuery
			{
				Name = request.Name
			};
			if (user.UserType != (int)UserType.TrainingOrganizationManager)
			{
				throw new PlatformException("该用户不是管理员");
			}
			query.TrainingOrganizationManageUserId = user.Id;
			if (request.TrainingOrganizationId != null)
			{
				query.TrainingOrganizationId = request.TrainingOrganizationId;
			}
			query.PageIndex = request.PageIndex;
			query.PageSize = request.PageSize;
			var queryResult = _trainingOrganizationClassService.Query(query);
			QueryResult<ClassDto> res = new QueryResult<ClassDto>
			{
				List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new ClassDto())).ToList(),
				Total = queryResult.Total
			};
			return this.Success(res);
		}


		/// <summary>
		/// 家长查询班级
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GenearchQuery(TrainingOrganizationClassQueryRequest request)
		{
			var user = UserAccount;
			var query = new TrainingOrganizationClassQuery
			{
				Name = request.Name
			};
			if (user.UserType != (int)UserType.TrainingOrganizationGenearch)
			{
				throw new PlatformException("该用户不是家长");
			}
			query.GenearchChildId = request.StudentId;
			if (request.TrainingOrganizationId != null)
			{
				query.TrainingOrganizationId = request.TrainingOrganizationId;
			}

			query.PageIndex = request.PageIndex;
			query.PageSize = request.PageSize;
			var queryResult = _trainingOrganizationClassService.Query(query);
			QueryResult<ClassDto> res = new QueryResult<ClassDto>
			{
				List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new ClassDto())).ToList(),
				Total = queryResult.Total
			};

			return this.Success(res);
		}

		/// <summary>
		/// 教师查询班级
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TeacherQuery(TrainingOrganizationClassQueryRequest request)
		{
			var user = UserAccount;
			var query = new TrainingOrganizationClassQuery
			{
				Name = request.Name
			};
			if (user.UserType != (int)UserType.TrainingOrganizationTeacher)
			{
				throw new PlatformException("该用户不是教师");
			}
			query.TrainingOrganizationTeacherId = user.Id;
			if (request.TrainingOrganizationId != null)
			{
				query.TrainingOrganizationId = request.TrainingOrganizationId;
			}
			query.PageIndex = request.PageIndex;
			query.PageSize = request.PageSize;
			var queryResult = _trainingOrganizationClassService.Query(query);
			QueryResult<ClassDto> res = new QueryResult<ClassDto>
			{
				List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new ClassDto())).ToList(),
				Total = queryResult.Total
			};

			return this.Success(res);
		}

		/// <summary>
		/// 新增班级
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Add([FromBody]TrainingOrganizationClassAddRequest request)
		{
			var user = UserAccount;
			_trainingOrganizationClassService.AddClass(user.Id, request);
			return this.Success();
		}

		/// <summary>
		/// 修改班级
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Update([FromBody]TrainingOrganizationClassUpdateRequest request)
		{
			var user = UserAccount;
			_trainingOrganizationClassService.UpdateClass(user.Id, request);
			return this.Success();
		}

		/// <summary>
		/// 删除一个班级
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Delete(long id)
		{
			var user = UserAccount;
			_trainingOrganizationClassService.DeleteClass(user.Id, id);
			return this.Success();
		}
	}
}
