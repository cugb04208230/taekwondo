using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Data
{
    /// <summary>
    /// 仓储接口定义
    /// </summary>
    public interface IRepository
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 定义泛型仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IRepository<TEntity, in TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>
    {
        #region GetById

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        #endregion

        #region FirstOrDefault

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

            #endregion

        #region Insert

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        #endregion

        #region Update

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        #endregion

        #region InsertOrUpdate

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// 更新或者新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        #endregion

        #region Delete

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        void Delete(Expression<Func<TEntity, bool>> where);

            #endregion

    }

    /// <inheritdoc />
    /// <summary>
    /// 默认long主键类型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : Entity
    {

    }


    /// <inheritdoc />
    /// <summary>
    /// 默认long主键类型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    public interface IQueryRepository<TEntity, in TQuery> : IRepository<TEntity, long> where TEntity : Entity where TQuery : BaseQuery<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        QueryResult<TEntity> Query(TQuery query);
    }

}
