using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Enums
{
    public enum QrCodeLoginStatus
    {
        [Description("生成")]
        Generate = 0,
        [Description("登陆成功")]
        Success = 1,
        [Description("已扫码")]
        Scan = 2,
        [Description("过期")]
        OutTime = 3,
        [Description("登陆异常")]
        Exception = 4,
    }
}
