using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// KoubeiAdvertDeliveryDiscountBatchqueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiAdvertDeliveryDiscountBatchqueryModel : AopObject
    {
        /// <summary>
        /// 媒体编号，使用前需要找业务申请 ，不申请直接调用会失败
        /// </summary>
        [XmlElement("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// 城市国标码，比如310100是上海
        /// </summary>
        [XmlElement("city_code")]
        public string CityCode { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [XmlElement("latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [XmlElement("longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// 输出的券列表大小
        /// </summary>
        [XmlElement("size")]
        public string Size { get; set; }

        /// <summary>
        /// 支付宝用户ID
        /// </summary>
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
