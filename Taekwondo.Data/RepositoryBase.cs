using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Data;
using Common.Extensions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Taekwondo.Data
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> 
        where TEntity : Entity<TPrimaryKey>
    {
        protected readonly DataBaseContext DbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        protected RepositoryBase(DataBaseContext dbContext)
        {
            DbContext = dbContext;
        }

        #region GetById

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public virtual TEntity Get(TPrimaryKey id)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        #endregion

        #region FirstOrDefault

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }


	    /// <summary>
	    /// 根据lambda表达式条件获取制定条件对象是否存在
	    /// </summary>
	    /// <param name="predicate">lambda表达式条件</param>
	    /// <returns></returns>
	    public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
	    {
		    return DbContext.Set<TEntity>().Any(predicate);
	    }

		#endregion

		#region LastOrDefault

		/// <summary>
		/// 根据lambda表达式条件获取单个实体
		/// </summary>
		/// <param name="predicate">lambda表达式条件</param>
		/// <returns></returns>
		public virtual TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate)
	    {
		    return DbContext.Set<TEntity>().LastOrDefault(predicate);
	    }

	    /// <summary>
	    /// 根据lambda表达式条件获取单个实体
	    /// </summary>
	    /// <param name="predicate">lambda表达式条件</param>
	    /// <returns></returns>
	    public virtual async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
	    {
		    return await DbContext.Set<TEntity>().LastOrDefaultAsync(predicate);
	    }

	    #endregion

		#region Insert

		/// <summary>
		/// 新增实体
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns></returns>
		public virtual TEntity Insert(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.LastModifiedAt = DateTime.Now;
            DbContext.Set<TEntity>().Add(entity);
            Save();
            return entity;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.LastModifiedAt = DateTime.Now;
            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        #endregion

        #region Update

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual TEntity Update(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            var obj = Get(entity.Id);
            EntityToEntity(entity, obj);
            Save();
            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity;
        }

        private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        {
            foreach (var mItem in typeof(T).GetProperties())
            {
                mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
            }
        }

        #endregion

        #region InsertOrUpdate

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            if (Get(entity.Id) != null)
                return Update(entity);
            entity.CreatedAt = DateTime.Now;
            return Insert(entity);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            if (await GetAsync(entity.Id) != null)
                return await UpdateAsync(entity);
            entity.CreatedAt = DateTime.Now;
            return Insert(entity);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public virtual void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            Save();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public virtual void Delete(TPrimaryKey id)
        {
            var model = Get(id);
            if (model == null)
            {
                throw new PlatformException("数据不存在，无法删除");
            }
            DbContext.Set<TEntity>().Remove(model);
            Save();
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            DbContext.Set<TEntity>().Where(where).ToList().ForEach(it => DbContext.Set<TEntity>().Remove(it));
            Save();
        }

        #endregion

        #region Private

        /// <summary>
        /// 事务性保存
        /// </summary>
        private void Save()
        {
            DbContext.SaveChanges();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        private static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        #endregion

        #region Batch

		/// <summary>
		/// 批量更新
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
        public bool BatchUpdate(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastModifiedAt = DateTime.Now;
                var entry = DbContext.Entry(entity);
                entry.State = EntityState.Modified;
            }
            return DbContext.SaveChanges()>0;
        }


        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int BatchInsert(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastModifiedAt = DateTime.Now;
                entity.CreatedAt = DateTime.Now;
            }
            DbContext.Set<TEntity>().AddRange(entities);
	        return DbContext.SaveChanges();
        }

		#endregion


		/// <summary>
		/// 根据lambda表达式条件获取单个实体
		/// </summary>
		/// <param name="predicate">lambda表达式条件</param>
		/// <returns></returns>
		public virtual List<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
		{
			return DbContext.Set<TEntity>().Where(predicate).ToList();
		}
	}
    
    /// <inheritdoc />
    /// <summary>
    /// 主键为long类型的仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TQuery">查询实体</typeparam>
    public abstract class QueryRepositoryBase<TEntity, TQuery> : RepositoryBase<TEntity,long> where TEntity : Entity<long> where TQuery : BaseQuery<TEntity>
    {
        protected QueryRepositoryBase(DataBaseContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual QueryResult<TEntity> Query(TQuery query)
        {
            var result = new QueryResult<TEntity> {Query = query};
            var queryable = Select(query).Where(Where(query));
            queryable = OrderBy(queryable, query);
            queryable = queryable.Skip(query.Skip ?? 0).Take(query.PageSize ?? 10);
            result.Total = Count(query);
            result.List = queryable.AsEnumerable();
            return result;
        }

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual int Count(TQuery query)
        {
            return Select(query).Where(Where(query)).Count();
        }

        /// <summary>
        /// 模型
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> Select(TQuery query)
        {
            return from item in DbContext.Set<TEntity>() select item;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, bool>> Where(TQuery query)
        {
            Expression<Func<TEntity, bool>> expression = item => item.Id > 0;
            if (query.Id.HasValue)
            {
                expression = expression.And(item => item.Id == query.Id);
            }
	        if (query.Ids != null && query.Ids.Any())
	        {
		        expression = expression.And(item => query.Ids.Contains(item.Id));
			}
	        return expression;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> OrderBy(IQueryable<TEntity> queryable, TQuery query)
        {
            return queryable.InternalOrderBy(query.OrderBys);
        }
    }
}