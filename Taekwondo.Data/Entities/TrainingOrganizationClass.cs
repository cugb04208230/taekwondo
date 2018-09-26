using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;
using Newtonsoft.Json;
using Taekwondo.Data.Enums;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构班级
    /// </summary>
    public class TrainingOrganizationClass:Entity
    {

        /// <summary>
        /// 培训机构Id
        /// </summary>
        public long TrainingOrganizationId { set; get; }
		
		/// <summary>
        /// 教师Id
        /// </summary>
        public long TrainingOrganizationTeacherId { set; get; }

		/// <summary>
		/// 培训机构班级名称
		/// </summary>
		[JsonProperty("name")]
		public string ClassName { set; get; }
	    /// <summary>
	    /// 隶属培训机构管理人员Id
	    /// </summary>
	    public long TrainingOrganizationManageUserId { set; get; }

	    /// <summary>
	    /// 段位
	    /// </summary>
	    public DanType Dan { set; get; }

		[NotMapped]
		public string OrgName { set; get; }

		#region Model
		/// <summary>
		/// 培训机构
		/// </summary>
		[NotMapped]
        public TrainingOrganization TrainingOrganization { set; get; }
		/// <summary>
		/// 培训机构科目
		/// </summary>
	    [NotMapped]
		public TrainingOrganizationSubject TrainingOrganizationSubject { set; get; }
		/// <summary>
		/// 培训机构科目班级的负责老师
		/// </summary>
	    [NotMapped]
		public TrainingOrganizationTeacher TrainingOrganizationTeacher { set; get; }

        #endregion
    }

	/// <summary>
	/// 培训机构班级查询
	/// </summary>
	public class TrainingOrganizationClassQuery : BaseQuery<TrainingOrganizationClass>
	{


		/// <summary>
		/// 培训机构Id
		/// </summary>
		public long? TrainingOrganizationId { set; get; }

		/// <summary>
		/// 教师Id
		/// </summary>
		public long? TrainingOrganizationTeacherId { set; get; }

		/// <summary>
		/// 隶属培训机构管理人员Id
		/// </summary>
		public long? TrainingOrganizationManageUserId { set; get; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long? GenearchChildId { set; get; }

		/// <summary>
		/// 培训机构班级名称
		/// </summary>
		public string Name { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationClassRepository : QueryRepositoryBase<TrainingOrganizationClass, TrainingOrganizationClassQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationClassRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		protected override IQueryable<TrainingOrganizationClass> Select(TrainingOrganizationClassQuery query)
		{
			var queryable = from cls in DbContext.Set<TrainingOrganizationClass>()
				join org in DbContext.Set<TrainingOrganization>() on cls.TrainingOrganizationId equals org.Id
				select new TrainingOrganizationClass
				{
					Id = cls.Id,
					TrainingOrganizationId = cls.TrainingOrganizationId,
					TrainingOrganizationTeacherId = cls.TrainingOrganizationTeacherId,
					OrgName = org.TrainingOrganizationName,
					ClassName = cls.ClassName,
					TrainingOrganizationManageUserId = cls.TrainingOrganizationManageUserId,
					Dan = cls.Dan
				};
			return queryable;
		}

		/// <inheritdoc />
		protected override Expression<Func<TrainingOrganizationClass, bool>> Where(TrainingOrganizationClassQuery query)
		{
			var ex= base.Where(query);
			if (query.TrainingOrganizationId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationId == query.TrainingOrganizationId);
			}
			if (query.TrainingOrganizationTeacherId.HasValue)
			{
				var classIds = DbContext.TrainingOrganizationClassTeacherMaps
					.Where(e => e.TrainingOrganizationTeacherId == query.TrainingOrganizationTeacherId)
					.Select(e => e.TrainingOrganizationClassId);
				ex = ex.And(e => classIds.Contains(e.Id));
			}
			if (query.Name.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.ClassName.Contains(query.Name));
			}
			if (query.TrainingOrganizationManageUserId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationManageUserId == query.TrainingOrganizationManageUserId);
			}
			if (query.GenearchChildId.HasValue)
			{
				var classIds = DbContext.TrainingOrganizationClassStudents.Where(e => e.GenearchChildId == query.GenearchChildId).Select(e=>e.TrainingOrganizationClassId);
				ex = ex.And(e => classIds.Contains(e.Id));
			}
			return ex;
		}
	}
}
