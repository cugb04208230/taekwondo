//using System;
//using System.Diagnostics;
//using Aop.Api;
//using Aop.Api.Request;
//using Aop.Api.Response;
//using Common.Extensions;
//using Common.Log;
//using Newtonsoft.Json;
//using NLog;
//
//namespace Common.Authorized.Pay
//{
//    public static class AliPay
//    {
//
//        private static string AppId => Settings.AlipayLoginAppId;
//        private static string Getway => Settings.AlipayGetway;
//        private static string PrivateKey => Settings.AlipayLoginPrivateKey;
//        private static string PublicKey => Settings.AlipayLoginPublickKey;
//        private static string RedirectBaseUrl => Settings.AlipayAuthCallBack;
//        private static string AlipayPublicKey => Settings.AlipayLoginAlipayPublickKey;
//
//        /// <summary>
//        /// 下单
//        /// </summary>
//        /// <returns></returns>
//        public static AlipayTradePrecreateResponse PlaceOrder(AlipayPlaceOrderDto dto)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayTradePrecreateRequest request =
//                new AlipayTradePrecreateRequest {BizContent = JsonConvert.SerializeObject(dto)};
//            //通过alipayClient调用API，获得对应的response类
//            AlipayTradePrecreateResponse response = alipayClient.Execute(request);
//            LogHelper.Init("AliPay").Info($"request type:[{dto.GetType().FullName}] with data:[{dto.SerializeObject()}],response:[{response.SerializeObject()}] with time :[{sw.ElapsedMilliseconds}] ");
//            //根据response中的结果继续业务逻辑处理
//            return response;
//        }
//        /// <summary>
//        /// 退款
//        /// </summary>
//        /// <returns></returns>
//        public static AlipayTradeRefundResponse TradeRefund(AlipayTradeRefundDto dto)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayTradeRefundRequest request =
//                new AlipayTradeRefundRequest { BizContent = JsonConvert.SerializeObject(dto)};
//            //通过alipayClient调用API，获得对应的response类
//            AlipayTradeRefundResponse response = alipayClient.Execute(request);
//            LogHelper.Init("AliPay").Info($"request type:[{dto.GetType().FullName}] with data:[{dto.SerializeObject()}],response:[{response.SerializeObject()}] with time :[{sw.ElapsedMilliseconds}] ");
//            //根据response中的结果继续业务逻辑处理
//            return response;
//        }
//
//        /// <summary>
//        /// 对账账单地址查询
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        public static AlipayDataDataserviceBillDownloadurlQueryResponse BillDownloadurl(AlipayBillDownloadurlDto dto)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayDataDataserviceBillDownloadurlQueryRequest request =
//                new AlipayDataDataserviceBillDownloadurlQueryRequest { BizContent = JsonConvert.SerializeObject(dto) };
//            //通过alipayClient调用API，获得对应的response类
//            AlipayDataDataserviceBillDownloadurlQueryResponse response = alipayClient.Execute(request);
//            LogHelper.Init("AliPay").Info($"request type:[{dto.GetType().FullName}] with data:[{dto.SerializeObject()}],response:[{response.SerializeObject()}] with time :[{sw.ElapsedMilliseconds}] ");
//            //根据response中的结果继续业务逻辑处理
//            return response;
//        }
//
//        /// <summary>
//        /// 订单查询
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        public static AlipayTradeQueryResponse TradeQuery(AlipayTradeQueryDto dto)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest{ BizContent = JsonConvert.SerializeObject(dto) }; 
//            AlipayTradeQueryResponse response = alipayClient.Execute(request);
//            LogHelper.Init("AliPay").Info($"request type:[{dto.GetType().FullName}] with data:[{dto.SerializeObject()}],response:[{response.SerializeObject()}] with time :[{sw.ElapsedMilliseconds}] ");
//            return response;
//        }
//
//        /// <summary>
//        /// 取消订单
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        public static AlipayTradeCancelResponse TradeCancel(AlipayTradeCancelDto dto)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppId, PrivateKey, "json", "1.0", "RSA2", AlipayPublicKey);
//            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest { BizContent = JsonConvert.SerializeObject(dto) };
//            AlipayTradeCancelResponse response = alipayClient.Execute(request);
//            LogHelper.Init("AliPay").Info($"request type:[{dto.GetType().FullName}] with data:[{dto.SerializeObject()}],response:[{response.SerializeObject()}] with time :[{sw.ElapsedMilliseconds}] ");
//            return response;
//        }
//    }
//
//    public class AlipayPlaceOrderDto
//    {
//        /// <summary>
//        /// 商户订单号,64个字符以内、只能包含字母、数字、下划线；需保证在商户端不重复
//        /// </summary>
//        [JsonProperty("out_trade_no")]
//        public string TradeNo { set; get; }
//
//        /// <summary>
//        /// 	订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果同时传入了【打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【打折金额】+【不可打折金额】
//        /// </summary>
//        [JsonProperty("total_amount")]
//        public decimal TotalAmount { set; get; }
//
//        /// <summary>
//        /// 订单标题	
//        /// </summary>
//        [JsonProperty("subject")]
//        public string Subject { set; get; }
//    }
//
//    public class AlipayTradeRefundDto
//    {
//
//        /// <summary>
//        /// 支付时传入的商户订单号，与trade_no必填一个
//        /// </summary>
//        [JsonProperty("out_trade_no")]
//        public string OrignalTradeId { set; get; }
//        /// <summary>
//        /// 支付时返回的支付宝交易号，与out_trade_no必填一个
//        /// </summary>
//        [JsonProperty("trade_no")]
//        public string AlipayTradeId { set; get; }
//        /// <summary>
//        /// 本次退款请求流水号，部分退款时必传
//        /// </summary>
//        [JsonProperty("out_request_no")]
//        public string TradeRefundTradeId { set; get; }
//        /// <summary>
//        /// 本次退款金额
//        /// </summary>
//        [JsonProperty("refund_amount")]
//        public string RefundAmount { set; get; }
//    }
//
//    public class AlipayBillDownloadurlDto
//    {
//        /// <summary>
//        /// 固定传入trade
//        /// </summary>
//        [JsonProperty("bill_type")]
//        public string BillType => "trade";
//        /// <summary>
//        /// 需要下载的账单日期，最晚是当期日期的前一天
//        /// </summary>
//        [JsonProperty("bill_date")]
//        public DateTime BillDate { set; get; }
//    }
//
//    public class AlipayTradeQueryDto
//    {
//        /// <summary>
//        /// 支付时传入的商户订单号，与trade_no必填一个
//        /// </summary>
//        [JsonProperty("out_trade_no")]
//        public string OrignalTradeId { set; get; }
//        /// <summary>
//        /// 支付时返回的支付宝交易号，与out_trade_no必填一个
//        /// </summary>
//        [JsonProperty("trade_no")]
//        public string AlipayTradeId { set; get; }
//    }
//
//    public class AlipayTradeCancelDto
//    {
//
//        /// <summary>
//        /// 原支付请求的商户订单号,和支付宝交易号不能同时为空
//        /// </summary>
//        [JsonProperty("out_trade_no")]
//        public string OrignalTradeId { set; get; }
//        /// <summary>
//        /// 支付宝交易号，和商户订单号不能同时为空
//        /// </summary>
//        [JsonProperty("trade_no")]
//        public string AlipayTradeId { set; get; }
//    }
//
//}
