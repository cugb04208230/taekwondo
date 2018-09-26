using Common.Data;
using Common.Di;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.TrainingOrganization;
using Taekwondo.Data.Entities;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers.Ent
{
	/// <inheritdoc />
	/// <summary>
	/// 俱乐部管理
	/// </summary>
	[Auth]
	[Route("api/ent/train/[action]")]
	public class EntTrainController:BaseController
	{
		private readonly DataBaseContext _dbContext;
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="dbContext"></param>
		public EntTrainController(DataBaseContext dbContext)
		{
			_dbContext = dbContext;
		}

        /// <summary>
        /// 获取俱乐部列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
		public IActionResult Trains(TrainQueryRequest request)
		{
            //http://localhost:52459/api/ent/train/Trains?Token=572f599495f24ed8a1bcc6d6f22a160b
            var query = new TrainingOrganizationEntQuery
			{
				Name = request.Name
			};
			var list = ObjectContainer.Instance.Resolver<TrainingOrganizationEntRepository>()
				.Query(query);
			return this.Success(list);
		}

	}
}
