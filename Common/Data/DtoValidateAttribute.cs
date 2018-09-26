using System;

namespace Common.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 数据模型验证
    /// </summary>
    public class DtoValidateAttribute:Attribute
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public int? Max { set; get; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int? Min { set; get; }

        /// <summary>
        /// 正则
        /// </summary>
        public string Regex { set; get; }

        /// <summary>
        /// 正则错误提示
        /// </summary>
        public string RegexNotice { set; get; }

        /// <summary>
        /// 枚举数组
        /// </summary>
        public int[] EnumArray { set; get; }
	    /// <summary>
	    /// 枚举类型
	    /// </summary>
	    public Type EnumType { set; get; }

		/// <summary>
		/// 最大长度
		/// </summary>
		public int MaxLength { set; get; }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength { set; get; }
    }
    
}
