using Common.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Common.Data
{
    public static class ControllerExtension
    {

        public static JsonResult Success<T>(this Controller me, T data)
        {
            return me.Result<T>(true,data);
        }

        public static JsonResult Success<T>(this Controller me)
        {
            return me.Result<T>(true);
        }

        public static JsonResult Success(this Controller me)
        {
            return me.Result(true);
        }

        public static JsonResult Fail(this Controller me, string message, int code = 200)
        {
            return me.Result(false,message,code);
        }

        public static JsonResult Result(this Controller me, bool success)
        {
            return me.Result<string>(success, null,null);
        }

        public static JsonResult Result<T>(this Controller me, bool success)
        {
            return me.Result<string>(success, null,null);
        }


        public static JsonResult Result<T>(this Controller me, bool success,string message)
        {
            return me.Result<string>(success,null, message);
        }

        public static JsonResult Result<T>(this Controller me,bool success, T data, int code = 200)
        {
            return me.Result<T>(success, data, "", code);
        }


        public static JsonResult Result<T>(this Controller me, bool success, T data, string message,int code=200)
        {
            var responseModel = new CommonResult<T>
            {
                Success = success,
                Data = data,
                Message = message,
                Code = code
            };
            LogManager.GetLogger("Result").Info(
                $"request:[{me.ControllerContext.HttpContext.Request}] ,response:[{responseModel}],ElapsedMilliseconds:");
            return new CustomJson(responseModel);
        }
    }
}
