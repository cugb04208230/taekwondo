using System;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构班级学生信息
    /// </summary>
    public class TrainingOrganizationClassStudent:Entity
	{
		/// <summary>
		/// 隶属培训机构管理人员Id
		/// </summary>
		public long TrainingOrganizationManageUserId { set; get; }
		/// <summary>
		/// 培训机构Id
		/// </summary>
		public long TrainingOrganizationId { set; get; }
		/// <summary>
		/// 培训机构班级信息
		/// </summary>
		public long TrainingOrganizationClassId { get; set; }

        /// <summary>
        /// 学员Id
        /// </summary>
        public long GenearchChildId { get; set; }

      

        /// <summary>
        /// 学员当前状态
        /// <see cref="Enums.TrainingOrganizationClassStudentStatus"/>
        /// </summary>
        public int TrainingOrganizationClassStudentStatus { set; get; }
    }

	/// <summary>
	/// 班级学员查询
	/// </summary>
	public class TrainingOrganizationClassStudentQuery : BaseQuery<TrainingOrganizationClassStudent>
	{
		/// <summary>
		/// 隶属培训机构管理人员Id
		/// </summary>
		public long? TrainingOrganizationManageUserId { set; get; }
		/// <summary>
		/// 培训机构Id
		/// </summary>
		public long? TrainingOrganizationId { set; get; }
		/// <summary>
		/// 班级Id
		/// </summary>
		public long? TrainingOrganizationClassId { get; set; }
		/// <summary>
		/// 学员Id
		/// </summary>
		public long? GenearchChildId { get; set; }

		/// <summary>
		/// 学员当前状态
		/// <see cref="Enums.TrainingOrganizationClassStudentStatus"/>
		/// </summary>
		public int? TrainingOrganizationClassStudentStatus { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationClassStudentRepository : QueryRepositoryBase<TrainingOrganizationClassStudent,
		TrainingOrganizationClassStudentQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationClassStudentRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<TrainingOrganizationClassStudent, bool>> Where(TrainingOrganizationClassStudentQuery query)
		{
			var ex=  base.Where(query);
			if (query.TrainingOrganizationManageUserId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationManageUserId == query.TrainingOrganizationManageUserId);
			}
			if (query.TrainingOrganizationId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationId == query.TrainingOrganizationId);
			}
			if (query.TrainingOrganizationClassId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationClassId == query.TrainingOrganizationClassId);
			}
			if (query.GenearchChildId.HasValue)
			{
				ex = ex.And(e => e.GenearchChildId == query.GenearchChildId);
			}
			if (query.TrainingOrganizationClassStudentStatus.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationClassStudentStatus == query.TrainingOrganizationClassStudentStatus);
			}
			return ex;
		}
	}
}
