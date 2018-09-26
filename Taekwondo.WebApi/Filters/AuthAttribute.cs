using Common.Di;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Taekwondo.Data.Enums;
using Taekwondo.Service;

namespace Taekwondo.WebApi.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// 授权验证
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
		/// <summary>
		/// 用户类型
		/// </summary>
		public UserType UserType { set; get; }

		/// <inheritdoc />
		/// <summary>
		/// 授权验证
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            var token = context.Request.Headers["token"].ToString();
	        if (token.IsNullOrEmpty())
			{
				token = context.Request.Query["token"].ToString();
			}
	        if (token.IsNullOrEmpty())
	        {
		        token = context.Request.Form["token"].ToString();
	        }
	        var userAccount = ObjectContainer.Instance.Resolver<AccountService>().CheckToken(token);
	        if (userAccount==null)
	        {
		        throw new PlatformException("无效的授权，请重新登录", 1001);
	        }
			((Controller)filterContext.Controller).ViewBag.User = userAccount;
		}
        
    }
}
