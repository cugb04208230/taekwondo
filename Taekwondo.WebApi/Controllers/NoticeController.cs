using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Di;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Notice;
using Taekwondo.Data.Entities;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 账号管理
	/// </summary>
	[Route("api/[controller]")]
	[Auth]
	public class NoticeController:BaseController
	{
		private readonly NoticeService _noticeService;
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="noticeService"></param>
	    public NoticeController(NoticeService noticeService)
	    {
		    _noticeService = noticeService;
	    }

	    /// <summary>
		/// 获取通知
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult Query(NoticeQueryRequest request)
	    {
		    var queryResult = _noticeService.Query(new NoticeQuery
		    {
			    UserId = UserAccountId,
				PageIndex = request.PageIndex,
				PageSize = request.PageSize,
				OrderBys = new List<OrderField>
				{
					new OrderField("IsRead",OrderDirection.Asc),
					new OrderField("Id",OrderDirection.Desc)
				}
		    });
		    var response = queryResult.List.Select(e => ModelMapUtil.AutoMap(e, new NoticeDto())).ToList();
		    response.ForEach(e =>
			{
				e.HeadPic = GetHeadPic(e.UserId);
				e.Title = "系统消息";

			});
			return this.Success(response);
	    }

		/// <summary>
		/// 读取消息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Read(long id)
		{
			_noticeService.ReadNotice(id);
			return this.Success();
		}

		/// <summary>
		/// 获取未读消息数量
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("Count")]
		public IActionResult Count()
		{
			var userId = UserAccountId;
			var count = ObjectContainer.Instance.Resolver<DataBaseContext>().Set<Notice>().Count(e => e.UserId == userId && !e.IsRead);
			return this.Success(count);
		}

	}
}
