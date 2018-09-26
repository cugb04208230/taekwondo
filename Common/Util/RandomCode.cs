using System;

namespace Common.Util
{
    public static class RandomCode
    {
        public static string Code(int length)
        {
            var num = new Random().Next(10000000, 99999999);
            return num.ToString().Substring(8-length);
        }
    }
}
