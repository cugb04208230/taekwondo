using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Log;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.MiddleWare
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        public Stopwatch Sw;

        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                Sw = new Stopwatch();
                Sw.Start();
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                Sw.Stop();
                if (!context.Request.Path.ToString().Contains("swagger"))
                {
                    LogHelper.Init("Timer").Info($"Action Time[Route:{context.Request.Path},QueryString:{context.Request.QueryString},time:{Sw.ElapsedMilliseconds}]");
                }
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            //记录日志
            LogHelper.Init("Error").Error(exception);
            //返回友好的提示
            var response = context.Response;
            //状态码
            var message = "服务器打了个盹~";
            var code = 200;
            if (exception is UnauthorizedAccessException)
                response.StatusCode = (int) HttpStatusCode.Unauthorized;
            if (exception is PlatformException platform)
            {
                message = exception.Message;
                code = platform.Code;
            }
            else if (exception != null)
                response.StatusCode = (int) HttpStatusCode.BadRequest;
            response.ContentType = context.Request.Headers["Accept"];
            response.ContentType = "application/json";
            await response.WriteAsync(JsonConvert.SerializeObject(new { success=false, message,code }))
                .ConfigureAwait(false);
        }
        

    }
}
