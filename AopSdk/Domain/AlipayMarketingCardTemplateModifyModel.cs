using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayMarketingCardTemplateModifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingCardTemplateModifyModel : AopObject
    {
        /// <summary>
        /// 业务卡号前缀，由商户自定义
        /// </summary>
        [XmlElement("biz_no_prefix")]
        public string BizNoPrefix { get; set; }

        /// <summary>
        /// 卡级别配置
        /// </summary>
        [XmlArray("card_level_conf")]
        [XmlArrayItem("template_card_level_conf_d_t_o")]
        public List<TemplateCardLevelConfDTO> CardLevelConf { get; set; }

        /// <summary>
        /// 栏位信息（卡包详情页面展现的栏位）
        /// </summary>
        [XmlArray("column_info_list")]
        [XmlArrayItem("template_column_info_d_t_o")]
        public List<TemplateColumnInfoDTO> ColumnInfoList { get; set; }

        /// <summary>
        /// 字段规则列表，会员卡开卡过程中，会员卡信息的生成规则，  例如：卡有效期为开卡后两年内有效，则设置为：DATE_IN_FUTURE
        /// </summary>
        [XmlArray("field_rule_list")]
        [XmlArrayItem("template_field_rule_d_t_o")]
        public List<TemplateFieldRuleDTO> FieldRuleList { get; set; }

        /// <summary>
        /// 商户动态码通知参数配置：  当write_off_type指定为商户动态码mdbarcode或mdqrcode时必填；  在此字段配置用户打开会员卡时支付宝通知商户生成动态码（发码）的通知参数，如接收通知地址等。
        /// </summary>
        [XmlElement("mdcode_notify_conf")]
        public TemplateMdcodeNotifyConfDTO MdcodeNotifyConf { get; set; }

        /// <summary>
        /// 会员卡用户领卡配置，在门店等渠道露出领卡入口时，需要部署的商户领卡H5页面地址
        /// </summary>
        [XmlElement("open_card_conf")]
        public TemplateOpenCardConfDTO OpenCardConf { get; set; }

        /// <summary>
        /// 卡模板投放渠道
        /// </summary>
        [XmlArray("pub_channels")]
        [XmlArrayItem("pub_channel_d_t_o")]
        public List<PubChannelDTO> PubChannels { get; set; }

        /// <summary>
        /// 请求ID，由开发者生成并保证唯一性
        /// </summary>
        [XmlElement("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// 会员卡上架门店id（支付宝门店id），既发放会员卡的商家门店id
        /// </summary>
        [XmlArray("shop_ids")]
        [XmlArrayItem("string")]
        public List<string> ShopIds { get; set; }

        /// <summary>
        /// 权益信息，  1、在卡包的卡详情页面会自动添加权益栏位，展现会员卡特权，  2、如果添加门店渠道，则可在门店页展现会员卡的权益
        /// </summary>
        [XmlArray("template_benefit_info")]
        [XmlArrayItem("template_benefit_info_d_t_o")]
        public List<TemplateBenefitInfoDTO> TemplateBenefitInfo { get; set; }

        /// <summary>
        /// 支付宝卡模板ID（模板创建接口返回的支付宝端模板ID）
        /// </summary>
        [XmlElement("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板样式信息
        /// </summary>
        [XmlElement("template_style_info")]
        public TemplateStyleInfoDTO TemplateStyleInfo { get; set; }

        /// <summary>
        /// 卡包详情页面中展现出的卡码（可用于扫码核销）  qrcode: 二维码  dqrcode: 动态二维码  barcode: 条码  dbarcode: 动态条码  text: 文本   mdbarcode: 商户动态条码  mdqrcode: 商户动态二维码
        /// </summary>
        [XmlElement("write_off_type")]
        public string WriteOffType { get; set; }
    }
}
