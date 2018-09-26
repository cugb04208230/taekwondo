using System.Linq;
using Common.Data;
using Common.Di;
using Common.Models;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data.DTOs.TrainingOrganization;
using Taekwondo.Data.Entities;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 管理员管理校区
	/// </summary>
	[Route("api/[controller]/[action]")]
	[Auth]
	public class TrainingOrganizationController:BaseController
	{
		private readonly TrainingOrganizationService _trainingOrganizationService;

		/// <summary>
		/// ctor.
		/// </summary>
		public TrainingOrganizationController(TrainingOrganizationService trainingOrganizationService)
		{
			_trainingOrganizationService = trainingOrganizationService;
		}

        /// <summary>
        /// 查询名下校区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
		public IActionResult Query(TrainingOrganizationQueryRequest request)
		{
			var query = new TrainingOrganizationQuery
			{
				Name = request.Name,
				TrainingOrganizationManagerUserId = UserAccountId
			};

            query.PageIndex = request.PageIndex;
            query.PageSize = request.PageSize;
            var queryResult = _trainingOrganizationService.Query(query);
		 
            QueryResult<TrainingOrganizationDto> res = new QueryResult<TrainingOrganizationDto>();
            res.Query = res.Query;
            res.List = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new TrainingOrganizationDto())).ToList();
            res.Total = queryResult.Total;
            return this.Success(res);
		}

        /// <summary>
        /// 新增一个校区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult Add(TrainingOrganizationAddRequest request)
		{
			var userId = UserAccountId ?? 0;
			var ent = ObjectContainer.Instance.Resolver<TrainingOrganizationEntRepository>()
				.FirstOrDefault(e => e.ManagerId == userId);
			if (ent == null)
			{
				throw new PlatformException("企业管理员才可以添加校区哦");
			}
			_trainingOrganizationService.Add(new TrainingOrganization{Summary = request.Name, TrainingOrganizationName = request.Name,TrainingOrganizationManagerUserId = ent.ManagerId, Address = request.Address});
			return this.Success();
		}

        /// <summary>
        /// 编辑一个校区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult Update(TrainingOrganizationUpdateRequest request)
		{
			var userId = UserAccountId ?? 0;
			var org = _trainingOrganizationService.Get(request.Id);
			if (org == null)
			{
				throw new PlatformException("企业管理员才可以编辑校区哦");
			}
			var ent = ObjectContainer.Instance.Resolver<TrainingOrganizationEntRepository>()
				.FirstOrDefault(e => e.ManagerId == userId);
			if (ent == null||userId!=org.TrainingOrganizationManagerUserId)
			{
				throw new PlatformException("企业管理员才可以编辑校区哦");
			}
			_trainingOrganizationService.Update(new TrainingOrganization { Id = request.Id,Summary = request.Name, TrainingOrganizationName = request.Name, TrainingOrganizationManagerUserId = userId, Address = request.Address });
			return this.Success();
		}

        /// <summary>
        /// 删除一个校区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult Delete(TrainingOrganizationDeleteRequest request)
		{
			var userId = UserAccountId ?? 0;
			var org = _trainingOrganizationService.Get(request.Id);
			if (org == null)
			{
				throw new PlatformException("企业管理员才可以编辑校区哦");
			}
			if (userId != org.TrainingOrganizationManagerUserId)
			{
				throw new PlatformException("企业管理员才可以编辑校区哦");
			}
			_trainingOrganizationService.Delete(request.Id);
			return this.Success();
		}
	}
}
