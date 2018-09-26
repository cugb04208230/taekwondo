using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构班级作业答题
    /// </summary>
    public class TrainingOrganizationClassHomeworkAnswer:Entity
    {
        /// <summary>
        /// 作业Id
        /// </summary>
        public long TrainingOrganizationClassHomeworkId { get; set; }

        /// <summary>
        /// 家长Id
        /// </summary>
        public long GenearchId { set; get; }

        /// <summary>
        /// 学员Id
        /// </summary>
        public long GenearchChildId { set; get; }

		/// <summary>
		/// 学员名称
		/// </summary>
		public string GenearchChildName { set; get; }

        /// <summary>
        /// 作业答题文字说明
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 作业答题视频说明
        /// </summary>
        public string Files { get; set; }

		/// <summary>
		/// 封面图片
		/// </summary>
		public string Images { set; get; }
//        /// <summary>
//        /// 作业答题视频说明
//        /// </summary>
//        public string Video { get; set; }
//
//        /// <summary>
//        /// 作业答题语音说明
//        /// </summary>
//        public string Voice { set; get; }

        /// <summary>
        /// 批阅状态
        /// </summary>
        public bool Readovered { set; get; }

        /// <summary>
        /// 批阅文字说明
        /// </summary>
        public string ReadoverText { get; set; }
//        /// <summary>
//        /// 批阅语音说明
//        /// </summary>
//        public string ReadoverVoice { set; get; }

		/// <summary>
		/// 评分
		/// </summary>
		public int Stars { set; get; }
		
    }

	/// <summary>
	/// 培训机构作业答题查询条件
	/// </summary>
	public class TrainingOrganizationClassHomeworkAnswerQuery:BaseQuery<TrainingOrganizationClassHomeworkAnswer>
	{
		/// <summary>
		/// 作业Id
		/// </summary>
		public long? TrainingOrganizationClassHomeworkId { get; set; }

		/// <summary>
		/// 家长Id
		/// </summary>
		public long? GenearchId { set; get; }

		/// <summary>
		/// 学员Id
		/// </summary>
		public long? GenearchChildId { set; get; }

		/// <summary>
		/// 作业列表
		/// </summary>
		public long[] HomeworkIds { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationClassHomeworkAnswerRepository : QueryRepositoryBase<
		TrainingOrganizationClassHomeworkAnswer, TrainingOrganizationClassHomeworkAnswerQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationClassHomeworkAnswerRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<TrainingOrganizationClassHomeworkAnswer, bool>> Where(TrainingOrganizationClassHomeworkAnswerQuery query)
		{
			var ex= base.Where(query);
			if (query.GenearchId.HasValue)
			{
				ex = ex.And(e => e.GenearchId == query.GenearchId);
			}
			if (query.GenearchChildId.HasValue)
			{
				ex = ex.And(e => e.GenearchChildId == query.GenearchChildId);
			}
			if (query.TrainingOrganizationClassHomeworkId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationClassHomeworkId == query.TrainingOrganizationClassHomeworkId);
			}
			if (query.HomeworkIds != null && query.HomeworkIds.Any())
			{
				ex = ex.And(e => query.HomeworkIds.Contains(e.TrainingOrganizationClassHomeworkId));
			}
			return ex;
		}
	}
}
