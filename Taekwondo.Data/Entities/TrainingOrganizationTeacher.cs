using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Data;
using Common.Extensions;
using Newtonsoft.Json;

namespace Taekwondo.Data.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// 培训机构教师
    /// </summary>
    public class TrainingOrganizationTeacher:Entity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
		public string TeacherName { set; get; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public long TeacherId { set; get; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }
		
        /// <summary>
        /// 隶属培训机构Id
        /// </summary>
        [JsonIgnore]
        public long TrainingOrganizationId { set; get; }
		/// <summary>
		/// 隶属培训机构管理人员Id
		/// </summary>
		[JsonIgnore]
		public long TrainingOrganizationManageUserId { set; get; }
	}

	/// <summary>
	/// 培训机构老师查询
	/// </summary>
	public class TrainingOrganizationTeacherQuery : BaseQuery<TrainingOrganizationTeacher>
	{

		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public long? TeacherId { set; get; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 隶属培训机构Id
		/// </summary>
		public long? TrainingOrganizationId { set; get; }

		/// <summary>
		/// 场馆管理人员ID
		/// </summary>
		public long? TrainingOrganizationManageUserId { get; set; }

		/// <summary>
		/// 老师Id数组
		/// </summary>
		public long[] TeacherIds { set; get; }
		/// <summary>
		/// 老师手机号或者名称
		/// </summary>
		public string CommonText { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class TrainingOrganizationTeacherRepository : QueryRepositoryBase<TrainingOrganizationTeacher,
			TrainingOrganizationTeacherQuery>
	{
		/// <inheritdoc />
		public TrainingOrganizationTeacherRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		/// <inheritdoc />
		protected override Expression<Func<TrainingOrganizationTeacher, bool>> Where(TrainingOrganizationTeacherQuery query)
		{
			var ex= base.Where(query);
			if (query.Mobile.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.Mobile.Contains(query.Mobile));
			}
			if (query.Name.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.TeacherName.Contains(query.Name));
			}
			if (query.CommonText.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.TeacherName.Contains(query.CommonText) || e.Mobile.Contains(query.CommonText));
			}
			if (query.TeacherId.HasValue)
			{
				ex = ex.And(e => e.TeacherId==query.TeacherId);
			}
			if (query.TrainingOrganizationId.HasValue)
			{
				ex = ex.And(e => e.TrainingOrganizationId==query.TrainingOrganizationId);
			}
			if (query.TrainingOrganizationManageUserId.HasValue)
			{
				ex = ex.And(e=>e.TrainingOrganizationManageUserId==query.TrainingOrganizationManageUserId);
			}
			if (query.TeacherIds != null && query.TeacherIds.Any())
			{
				ex = ex.And(item => query.TeacherIds.Contains(item.TeacherId));
			}
			return ex;
		}
	}
}
