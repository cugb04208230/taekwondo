using System;

namespace Common.Data
{

    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// 定义默认主键类型为long的实体基类
    /// </summary>
    public abstract class Entity : Entity<long>
    {
    }

}
