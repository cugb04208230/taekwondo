using System;
using Common.Extensions;
using Taekwondo.Data;

namespace Taekwondo.Service
{
    public class BaseService
    {
	    protected readonly DataBaseContext DbContext;
		public BaseService(DataBaseContext dbContext)
		{
			DbContext = dbContext;
		}


		//	    private string GetIp()
		//	    {
		//		    var xfor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault(); ;
		//		    if (!string.IsNullOrEmpty(xfor))
		//		    {
		//			    return xfor;
		//		    }
		//		    var realRemoteIp = Request.Headers["REMOTE_ADDR"].FirstOrDefault();
		//		    if (!string.IsNullOrEmpty(realRemoteIp))
		//		    {
		//			    return realRemoteIp;
		//		    }
		//		    return Request.Host.Host;
		//	    }


		protected DateTime CtorTime(DateTime day, string time)
		{
			return new DateTime(day.Year, day.Month, day.Day, time.Split(":")[0].To(0), time.Split(":")[1].To(0), 0);
		}

	}
}
