using System.Linq;
using Common.Data;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.Genearch;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 家长管理
	/// </summary>
	[Auth]
	[Route("api/[controller]/[action]")]
	public class GenearchController : BaseController
	{
		private readonly GenearchService _genearchService;



		/// <summary>
		/// ctor.
		/// </summary>
		public GenearchController(GenearchService genearchService)
		{
            _genearchService = genearchService;
		}

		/// <summary>
		/// 获取家长列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult Query(GenearchQueryRequest request)
		{
			var query = new GenearchQuery {ClassId = request.ClassId};
			var user = UserAccount;
			if (user.UserType == (int) UserType.TrainingOrganizationManager)//如果是场馆管理人员
			{
				query.TrainingOrganizationId = user.Id;
			 
			}
            query.PageIndex = request.PageIndex;
            query.PageSize = request.PageSize;
            //if (user.UserType == (int) UserType.TrainingOrganizationTeacher)//如果是场馆的教师
            //{
            //	query.TeacherId = user.Id;
            //}
            //if (user.UserType == (int) UserType.TrainingOrganizationGenearch)
            //{
            //	query.GenearchId = user.Id;
            //}
            var queryResult = _genearchService.QueryGenearch(query);
		 
		 
            QueryResult<GenearchDto> res = new QueryResult<GenearchDto>();

            res.List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new GenearchDto())).ToList();
            res.Total = queryResult.Total;

            return this.Success(res);
		}

		/// <summary>
		/// 新增家长
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Add(GenearchAddRequest request)
		{
            _genearchService.AddGenearch(UserAccountId??0,request);
			return this.Success();
		}

        /// <summary>
        /// 删除家长
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(long id)
        {
            var user = UserAccount;
            _genearchService.DeleteGenearch(user.Id, id);
            return this.Success();
        }

        /// <summary>
		/// 修改家长
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
        public IActionResult Update(GenearchUpdateRequest request)
        {
            _genearchService.UpdateGenearch(UserAccountId ?? 0, request);
            return this.Success();
        }
    }
}
