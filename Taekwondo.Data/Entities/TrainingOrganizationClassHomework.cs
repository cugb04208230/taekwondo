using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构班级作业
    /// </summary>
    public class TrainingOrganizationClassHomework:Entity
    {
        /// <summary>
        /// 培训机构Id
        /// </summary>
        public long TrainingOrganizationId { get; set; }

        /// <summary>
        /// 培训机构班级Id
        /// </summary>
        public long TrainingOrganizationClassId { get; set; }

        /// <summary>
        /// 培训机构班级老师Id
        /// </summary>
        public long TrainingOrganizationClassTeacherId { get; set; }
	    /// <summary>
	    /// 隶属培训机构管理人员Id
	    /// </summary>
	    public long TrainingOrganizationManageUserId { set; get; }

		/// <summary>
		/// 作业标题
		/// </summary>
		public string Title { set; get; }

        /// <summary>
        /// 作业文字说明
        /// </summary>
        public string Summary { get; set; }

//        /// <summary>
//        /// 作业图片说明
//        /// </summary>
//        public string Picture { get; set; }

        /// <summary>
        /// 作业视频说明
        /// </summary>
        public string Files { get; set; }
	    /// <summary>
	    /// 封面图片
	    /// </summary>
	    public string Images { set; get; }

		//        /// <summary>
		//        /// 作业语音说明
		//        /// </summary>
		//        public string Voice { set; get; }

		/// <summary>
		/// 答题列表（默认10条，答题时间正序）
		/// </summary>
		[NotMapped]
		public List<TrainingOrganizationClassHomeworkAnswer> Answers { get; set; }
    }

	/// <summary>
	/// 作业查询条件
	/// </summary>
	public class TrainingOrganizationClassHomeworkQuery : BaseQuery<TrainingOrganizationClassHomework>
	{
		/// <summary>
		/// 培训机构Id
		/// </summary>
		public long? TrainingOrganizationId { get; set; }

		/// <summary>
		/// 培训机构班级Id
		/// </summary>
		public long[] TrainingOrganizationClassId { get; set; }

		/// <summary>
		/// 培训机构班级老师Id
		/// </summary>
		public long? TrainingOrganizationClassTeacherId { get; set; }

		/// <summary>
		/// 家长查询时使用
		/// </summary>
		public bool? IsFinished { get; set; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long? GenearchChildId { set; get; }

		/// <summary>
		/// 作业标题
		/// </summary>
		public string Title { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationClassHomeworkRepository : QueryRepositoryBase<TrainingOrganizationClassHomework,
		TrainingOrganizationClassHomeworkQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationClassHomeworkRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<TrainingOrganizationClassHomework, bool>> Where(TrainingOrganizationClassHomeworkQuery query)
		{
			var ex = base.Where(query);
			if (query.Title.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.Title.Contains(query.Title));
			}
			if (query.TrainingOrganizationId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationId == query.TrainingOrganizationId);
			}
			if (query.TrainingOrganizationClassId!=null&& query.TrainingOrganizationClassId.Any())
			{
				ex = ex.And(e => query.TrainingOrganizationClassId.Contains(e.TrainingOrganizationClassId));
			}
			if (query.TrainingOrganizationClassTeacherId.HasValue)
			{
				var clsIds = DbContext.Set<TrainingOrganizationClassTeacherMap>().Where(e =>
					e.TrainingOrganizationTeacherId == query.TrainingOrganizationClassTeacherId).Select(e=>e.TrainingOrganizationClassId).ToList();
				ex = ex.And(e => clsIds.Contains(e.TrainingOrganizationClassId));
			}
			if (query.GenearchChildId.HasValue)
			{
				var classIds = DbContext.TrainingOrganizationClassStudents.Where(e => e.GenearchChildId == query.GenearchChildId)
					.Select(e => e.TrainingOrganizationClassId);
				ex = ex.And(e => classIds.Contains(e.TrainingOrganizationClassId));
			}
			return ex;
		}
	}
}
