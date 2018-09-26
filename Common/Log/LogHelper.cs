using System;
using Newtonsoft.Json;
using NLog;

namespace Common.Log
{
    public static class LogHelper
    {

        public static ILogger Logger = Init("Application");


        public static void Info(object obj)
        {
            Logger.Info(JsonConvert.SerializeObject(obj));
        }

        public static void Error(Exception e)
        {
            Logger.Error(e);
        }

        public static ILogger Init(string name)
        {
            return LogManager.GetLogger(name);
        }

        public static void Info(this Logger logger,object obj)
        {
            logger.Info(JsonConvert.SerializeObject(obj));
        }

        public static void Error(this Logger logger, Exception e)
        {
            logger.Error(e);
        }
    }
}
