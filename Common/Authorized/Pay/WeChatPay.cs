using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Common.Log;
using Common.Models;
using Microsoft.AspNetCore.Http;

namespace Common.Authorized.Pay
{
    public static class WeChatPay
    {
        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
        public static string GetPrePayUrl(string productId)
        {
            LogHelper.Logger.Info("Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.Appid); //公众帐号id
            data.SetValue("mch_id", WxPayConfig.Mchid); //商户号
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp()); //时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr()); //随机字符串
            data.SetValue("product_id", productId); //商品ID
            data.SetValue("sign", data.MakeSign()); //签名
            string str = ToUrlParams(data.GetValues()); //转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            LogHelper.Logger.Info("Get native pay mode 1 url : " + url);
            return url;
        }

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public static string GetPayUrl(string productId)
        {
            LogHelper.Logger.Info("Native pay mode 2 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("body", "test"); //商品描述
            data.SetValue("attach", "test"); //附加数据
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo()); //随机字符串
            data.SetValue("total_fee", 1); //总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss")); //交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss")); //交易结束时间
            data.SetValue("goods_tag", "jjj"); //商品标记
            data.SetValue("trade_type", "NATIVE"); //交易类型
            data.SetValue("product_id", productId); //商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data); //调用统一下单接口
            string url = result.GetValue("code_url").ToString(); //获得统一下单接口返回的二维码链接

            LogHelper.Logger.Info("Get native pay mode 2 url : " + url);
            return url;
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private static string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }

    public class WxPayApi
    {
        /**
        * 提交被扫支付API
        * 收银员使用扫码设备读取微信用户刷卡授权码以后，二维码或条码信息传送至商户收银台，
        * 由商户收银台或者商户后台调用该接口发起支付。
        * @param WxPayData inputObj 提交给被扫支付API的参数
        * @param int timeOut 超时时间
        * @throws PlatformException
        * @return 成功时返回调用结果，其他抛异常
        */
        public static WxPayData Micropay(WxPayData inputObj, int timeOut = 10)
        {
            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            //检测必填参数
            if (!inputObj.IsSet("body"))
            {
                throw new  PlatformException ("提交被扫支付API接口中，缺少必填参数body！");
            }
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new PlatformException("提交被扫支付API接口中，缺少必填参数out_trade_no！");
            }
            if (!inputObj.IsSet("total_fee"))
            {
                throw new PlatformException("提交被扫支付API接口中，缺少必填参数total_fee！");
            }
            if (!inputObj.IsSet("auth_code"))
            {
                throw new PlatformException("提交被扫支付API接口中，缺少必填参数auth_code！");
            }

            inputObj.SetValue("spbill_create_ip", WxPayConfig.Ip);//终端ip
            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            LogHelper.Logger.Debug("WxPayApi:{0}", "MicroPay request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            LogHelper.Logger.Debug("WxPayApi:{0}", "MicroPay response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        *    
        * 查询订单
        * @param WxPayData inputObj 提交给查询订单API的参数
        * @param int timeOut 超时时间
        * @throws PlatformException
        * @return 成功时返回订单查询结果，其他抛异常
        */
        public static WxPayData OrderQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new PlatformException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            LogHelper.Logger.Debug("WxPayApi:{0}", "OrderQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口提交数据
            LogHelper.Logger.Debug("WxPayApi:{0}", "OrderQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 撤销订单API接口
        * @param WxPayData inputObj 提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个
        * @param int timeOut 接口超时时间
        * @throws PlatformException
        * @return 成功时返回API调用结果，其他抛异常
        */
        public static WxPayData Reverse(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new PlatformException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            LogHelper.Logger.Debug("WxPayApi:{0}", "Reverse request : " + xml);

            string response = HttpService.Post(xml, url, true, timeOut);

            LogHelper.Logger.Debug("WxPayApi:{0}", "Reverse response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 申请退款
        * @param WxPayData inputObj 提交给申请退款API的参数
        * @param int timeOut 超时时间
        * @throws PlatformException
        * @return 成功时返回接口调用结果，其他抛异常
        */
        public static WxPayData Refund(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new PlatformException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new PlatformException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new PlatformException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new PlatformException("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new PlatformException("退款申请接口中，缺少必填参数op_user_id！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            LogHelper.Logger.Debug("WxPayApi:{0}", "Refund request : " + xml);
            string response = HttpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API
            LogHelper.Logger.Debug("WxPayApi:{0}", "Refund response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
	    * 
	    * 查询退款
	    * 提交退款申请后，通过该接口查询退款状态。退款有一定延时，
	    * 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
	    * out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
	    * @param WxPayData inputObj 提交给查询退款API的参数
	    * @param int timeOut 接口超时时间
	    * @throws PlatformException
	    * @return 成功时返回，其他抛异常
	    */
        public static WxPayData RefundQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            //检测必填参数
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new PlatformException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            LogHelper.Logger.Debug("WxPayApi:{0}", "RefundQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            LogHelper.Logger.Debug("WxPayApi:{0}", "RefundQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 下载对账单
        * @param WxPayData inputObj 提交给下载对账单API的参数
        * @param int timeOut 接口超时时间
        * @throws PlatformException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData DownloadBill(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            //检测必填参数
            if (!inputObj.IsSet("bill_date"))
            {
                throw new PlatformException("对账单接口中，缺少必填参数bill_date！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            LogHelper.Logger.Debug("WxPayApi:{0}", "DownloadBill request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            LogHelper.Logger.Debug("WxPayApi:{0}", "DownloadBill result : " + response);

            WxPayData result = new WxPayData();
            //若接口调用失败会返回xml格式的结果
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //接口调用成功则返回非xml格式的数据
            else
                result.SetValue("result", response);

            return result;
        }


        /**
	    * 
	    * 转换短链接
	    * 该接口主要用于扫码原生支付模式一中的二维码链接转成短链接(weixin://wxpay/s/XXXXXX)，
	    * 减小二维码数据量，提升扫描速度和精确度。
	    * @param WxPayData inputObj 提交给转换短连接API的参数
	    * @param int timeOut 接口超时时间
	    * @throws PlatformException
	    * @return 成功时返回，其他抛异常
	    */
        public static WxPayData ShortUrl(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            //检测必填参数
            if (!inputObj.IsSet("long_url"))
            {
                throw new PlatformException("需要转换的URL，签名用原串，传输需URL encode！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串	
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            LogHelper.Logger.Debug("WxPayApi:{0}", "ShortUrl request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            LogHelper.Logger.Debug("WxPayApi:{0}", "ShortUrl response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws PlatformException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new PlatformException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new PlatformException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new PlatformException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new PlatformException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new PlatformException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new PlatformException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", WxPayConfig.NotifyUrl);//异步通知url
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("spbill_create_ip", WxPayConfig.Ip);//终端ip	  	    
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign());
            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            LogHelper.Logger.Debug("WxPayApi:{0}", "UnfiedOrder request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            LogHelper.Logger.Debug("WxPayApi:{0}", "UnfiedOrder response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
	    * 
	    * 关闭订单
	    * @param WxPayData inputObj 提交给关闭订单API的参数
	    * @param int timeOut 接口超时时间
	    * @throws PlatformException
	    * @return 成功时返回，其他抛异常
	    */
        public static WxPayData CloseOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new PlatformException("关闭订单接口中，out_trade_no必填！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串		
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
	    * 
	    * 测速上报
	    * @param string interface_url 接口URL
	    * @param int timeCost 接口耗时
	    * @param WxPayData inputObj参数数组
	    */
        private static void ReportCostTime(string interfaceUrl, int timeCost, WxPayData inputObj)
        {
            //如果不需要进行上报

            //如果仅失败上报
            if (WxPayConfig.ReportLevenl == 1 && inputObj.IsSet("return_code") && inputObj.GetValue("return_code").ToString() == "SUCCESS" &&
                inputObj.IsSet("result_code") && inputObj.GetValue("result_code").ToString() == "SUCCESS")
            {
                return;
            }

            //上报逻辑
            WxPayData data = new WxPayData();
            data.SetValue("interface_url", interfaceUrl);
            data.SetValue("execute_time_", timeCost);
            //返回状态码
            if (inputObj.IsSet("return_code"))
            {
                data.SetValue("return_code", inputObj.GetValue("return_code"));
            }
            //返回信息
            if (inputObj.IsSet("return_msg"))
            {
                data.SetValue("return_msg", inputObj.GetValue("return_msg"));
            }
            //业务结果
            if (inputObj.IsSet("result_code"))
            {
                data.SetValue("result_code", inputObj.GetValue("result_code"));
            }
            //错误代码
            if (inputObj.IsSet("err_code"))
            {
                data.SetValue("err_code", inputObj.GetValue("err_code"));
            }
            //错误代码描述
            if (inputObj.IsSet("err_code_des"))
            {
                data.SetValue("err_code_des", inputObj.GetValue("err_code_des"));
            }
            //商户订单号
            if (inputObj.IsSet("out_trade_no"))
            {
                data.SetValue("out_trade_no", inputObj.GetValue("out_trade_no"));
            }
            //设备号
            if (inputObj.IsSet("device_info"))
            {
                data.SetValue("device_info", inputObj.GetValue("device_info"));
            }

            try
            {
                Report(data);
            }
            catch (PlatformException ex)
            {
                LogHelper.Logger.Info(ex);
                //不做任何处理
            }
        }


        /**
	    * 
	    * 测速上报接口实现
	    * @param WxPayData inputObj 提交给测速上报接口的参数
	    * @param int timeOut 测速上报接口超时时间
	    * @throws PlatformException
	    * @return 成功时返回测速上报接口返回的结果，其他抛异常
	    */
        public static WxPayData Report(WxPayData inputObj, int timeOut = 1)
        {
            string url = "https://api.mch.weixin.qq.com/payitil/report";
            //检测必填参数
            if (!inputObj.IsSet("interface_url"))
            {
                throw new PlatformException("接口URL，缺少必填参数interface_url！");
            }
            if (!inputObj.IsSet("return_code"))
            {
                throw new PlatformException("返回状态码，缺少必填参数return_code！");
            }
            if (!inputObj.IsSet("result_code"))
            {
                throw new PlatformException("业务结果，缺少必填参数result_code！");
            }
            if (!inputObj.IsSet("user_ip"))
            {
                throw new PlatformException("访问接口IP，缺少必填参数user_ip！");
            }
            if (!inputObj.IsSet("execute_time_"))
            {
                throw new PlatformException("接口耗时，缺少必填参数execute_time_！");
            }

            inputObj.SetValue("appid", WxPayConfig.Appid);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.Mchid);//商户号
            inputObj.SetValue("user_ip", WxPayConfig.Ip);//终端ip
            inputObj.SetValue("time", DateTime.Now.ToString("yyyyMMddHHmmss"));//商户上报时间	 
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            LogHelper.Logger.Info("WxPayApi:{0}", "Report request : " + xml);

            string response = HttpService.Post(xml, url, false, timeOut);

            LogHelper.Logger.Info("WxPayApi:{0}", "Report response : " + response);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WxPayConfig.Mchid, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return System.Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }

    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string Appid = "wx2428e34e0e7dc6ef";
        public const string Mchid = "1233410002";
        public const string Key = "e10adc3849ba56abbe56e056f20f883e";
        public const string Appsecret = "51c56b886b5be869567dd389b3e5d1d6";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SslcertPath = "cert/apiclient_cert.p12";
        public const string SslcertPassword = "1233410002";

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NotifyUrl = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string Ip = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string ProxyUrl = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int ReportLevenl = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LogLevenl = 0;
    }

    /// <summary>
    /// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
    /// 在调用接口之前先填充各个字段的值，然后进行接口通信，
    /// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
    /// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
    /// </summary>
    public class WxPayData
    {
        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private readonly SortedDictionary<string, object> _mValues = new SortedDictionary<string, object>();

        /**
        * 设置某个字段的值
        * @param key 字段名
         * @param value 字段值
        */
        public void SetValue(string key, object value)
        {
            _mValues[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public object GetValue(string key)
        {
            object o;
            _mValues.TryGetValue(key, out o);
            return o;
        }

        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            object o;
            _mValues.TryGetValue(key, out o);
            if (null != o)
                return true;
            return false;
        }

        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws PlatformException
        **/
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == _mValues.Count)
            {
                throw new PlatformException("WxPayData数据为空!");
            }
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    throw new PlatformException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Value is int)
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value is string)
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    throw new PlatformException("WxPayData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /**
        * @将xml转为WxPayData对象并返回对象内部的数据
        * @param string 待转换的xml串
        * @return 经转换得到的Dictionary
        * @throws PlatformException
        */
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new PlatformException("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                _mValues[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            try
            {
                //2015-06-29 错误是没有签名
                if ((string)_mValues["return_code"] != "SUCCESS")
                {
                    return _mValues;
                }
                CheckSign();//验证签名,不通过会抛异常
            }
            catch (PlatformException ex)
            {
                throw new PlatformException(ex.Message);
            }

            return _mValues;
        }

        /**
        * @Dictionary格式转化成url参数格式
        * @ return url格式串, 该串不包含sign字段值
        */
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                if (pair.Value == null)
                {
                    throw new PlatformException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }


        /**
        * @Dictionary格式化成Json
         * @return json串数据
        */
        public string ToJson()
        {
//            string jsonStr = JsonMapper.ToJson(_mValues);
//            return jsonStr;
            return string.Empty;
        }

        /**
        * @values格式化成能在Web页面上显示的结果（因为web页面上不能直接输出xml格式的字符串）
        */
        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in _mValues)
            {
                if (pair.Value == null)
                {
                    throw new PlatformException("WxPayData内部含有值为null的字段!");
                }
                str += $"{pair.Key}={pair.Value}<br>";
            }
            LogHelper.Logger.Debug(GetType().ToString(), "Print in Web Page : " + str);
            return str;
        }

        /**
        * @生成签名，详见签名生成算法
        * @return 签名, sign字段不参加签名
        */
        public string MakeSign()
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + WxPayConfig.Key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /**
        * 
        * 检测签名是否正确
        * 正确返回true，错误抛异常
        */
        public bool CheckSign()
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                throw new PlatformException("WxPayData签名存在但不合法!");
            }
            //如果设置了签名但是签名为空，则抛异常
            if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                throw new PlatformException("WxPayData签名存在但不合法!");
            }

            //获取接收到的签名
            string returnSign = GetValue("sign").ToString();

            //在本地计算新的签名
            string calSign = MakeSign();

            if (calSign == returnSign)
            {
                return true;
            }

            throw new PlatformException("WxPayData签名验证错误!");
        }

        /**
        * @获取Dictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return _mValues;
        }
    }

    /// <summary>
    /// http连接基础类，负责底层的http通信
    /// </summary>
    public class HttpService
    {

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        public static string Post(string xml, string url, bool isUseCert, int timeout)
        {
            GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =CheckValidationResult;
                }
                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 1000;
                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;
                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = AppDomain.CurrentDomain.RelativeSearchPath;
                    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SslcertPath, WxPayConfig.SslcertPassword);
                    request.ClientCertificates.Add(cert);
                    LogHelper.Logger.Debug("WxPayApi:{0}", "PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogHelper.Logger.Error("HttpService:{0}", "Thread - caught ThreadAbortException - resetting.");
                LogHelper.Logger.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogHelper.Logger.Error("HttpService:{0}", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    LogHelper.Logger.Error("HttpService:{0}", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new PlatformException(e.ToString());
            }
            catch (Exception e)
            {
                throw new PlatformException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =CheckValidationResult;
                }
                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogHelper.Logger.Error("HttpService:{0}", "Thread - caught ThreadAbortException - resetting.");
                LogHelper.Logger.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogHelper.Logger.Error("HttpService:{0}", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    LogHelper.Logger.Error("HttpService:{0}", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new PlatformException(e.ToString());
            }
            catch (Exception e)
            {
                throw new PlatformException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                response?.Close();
                request?.Abort();
            }
            return result;
        }
    }
}
