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
    /// 家长关系绑定的小孩儿
    /// </summary>
    public class GenearchChild:Entity
    {

	    /// <summary>
	    /// 家长Id
	    /// </summary>
	    public long GenearchId { get; set; }

	    /// <summary>
	    /// 称谓
	    /// </summary>
	    public string Appellation { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		[JsonProperty("name")]
		public string GenearchChildName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }

		/// <summary>
		/// 段位
		/// </summary>
		public DanType Dan { set; get; }

        /// <summary>
        /// 剩余课程
        /// </summary>
        public long? LessionRemain { get; set; }

		/// <summary>
		/// 班级名称
		/// </summary>
		[NotMapped]
		public string ClassName { set; get; }

		/// <summary>
		/// 校区名称
		/// </summary>
		[NotMapped]
		public string TrainingOrganizationName { set; get; }

    }

	/// <summary>
	/// 家长学员信息查询
	/// </summary>
	public class GenearchChildQuery : BaseQuery<GenearchChild>
	{

		/// <summary>
		/// 家长Id
		/// </summary>
		public long? GenearchId { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 班级Id
		/// </summary>
		public long? ClassId { get; set; }

		/// <summary>
		/// 老师Id
		/// </summary>
		public long? TeacherId { get; set; }

		/// <summary>
		/// 场馆Id
		/// </summary>
		public long? TrainingOrganizationId { get; set; }

		/// <summary>
		/// 场馆管理人员ID
		/// </summary>
		public long? TrainingOrganizationManageUserId { get; set; }
	}

	/// <inheritdoc />
	/// <summary>
	/// 家长下的学员
	/// </summary>
	public class GenearchChildRepository : QueryRepositoryBase<GenearchChild, GenearchChildQuery>
	{
		/// <inheritdoc />
		public GenearchChildRepository(DataBaseContext dbContext) : base(dbContext)
		{
		}

		protected override IQueryable<GenearchChild> Select(GenearchChildQuery query)
		{
			if (query.GenearchId.HasValue)
			{
				return from map in DbContext.Set<GenearchChildMap>().Where(e => e.GenearchId == query.GenearchId)
					join genearchChild in DbContext.Set<GenearchChild>() on map.GenearchChildId equals genearchChild.Id
					select new GenearchChild
					{
						Id = genearchChild.Id,
						Appellation = map.Appellation,
						CreatedAt = genearchChild.CreatedAt,
						GenearchChildName = genearchChild.GenearchChildName,
						GenearchId = map.GenearchId,
						IdCardNo = genearchChild.IdCardNo,
						LastModifiedAt = genearchChild.LastModifiedAt,
						Dan = genearchChild.Dan
					};
			}
			return from map in DbContext.Set<GenearchChildMap>()
				join genearchChild in DbContext.Set<GenearchChild>() on map.GenearchChildId equals genearchChild.Id
				select new GenearchChild
				{
					Id = genearchChild.Id,
					Appellation = map.Appellation,
					CreatedAt = genearchChild.CreatedAt,
					GenearchChildName = genearchChild.GenearchChildName,
					GenearchId = map.GenearchId,
					IdCardNo = genearchChild.IdCardNo,
					LastModifiedAt = genearchChild.LastModifiedAt,
					Dan = genearchChild.Dan
				};
		}


		/// <inheritdoc />
		protected override Expression<Func<GenearchChild, bool>> Where(GenearchChildQuery query)
		{
			var ex= base.Where(query);
			if (query.GenearchId.HasValue)
			{
				var genearchChildIds = DbContext.GenearchChildMaps.Where(e => e.GenearchId == query.GenearchId).Select(e=>e.GenearchChildId);
				ex = ex.And(e => genearchChildIds.Contains(e.Id));
			}
			if (query.Name.IsNotNullOrEmpty())
			{
				ex = ex.And(e => e.GenearchChildName.Contains(query.Name));
			}
			if (query.ClassId.HasValue)
			{
				var studentIds = DbContext.TrainingOrganizationClassStudents.Where(e =>
					e.TrainingOrganizationClassId == query.ClassId ).Select(e => e.GenearchChildId);
				ex = ex.And(e => studentIds.Contains(e.Id));
			}
			if (query.TeacherId.HasValue)
			{
				var classIds =
					DbContext.TrainingOrganizationClassTeacherMaps.Where(e => e.TrainingOrganizationTeacherId == query.TeacherId).Select(e => e.TrainingOrganizationClassId);
				var studentIds = DbContext.TrainingOrganizationClassStudents.Where(e =>
					classIds.Contains(e.TrainingOrganizationClassId)).Select(e => e.GenearchChildId);
				ex = ex.And(e => studentIds.Contains(e.Id));
			}
			if (query.TrainingOrganizationId.HasValue)
			{
				var classIds = DbContext.TrainingOrganizationClasses
					.Where(e => e.TrainingOrganizationId == query.TrainingOrganizationId).Select(e => e.Id);
				var studentIds = DbContext.TrainingOrganizationClassStudents.Where(e =>
					classIds.Contains(e.TrainingOrganizationClassId)).Select(e => e.GenearchChildId);
				ex = ex.And(e => studentIds.Contains(e.Id));
			}
			if (query.TrainingOrganizationManageUserId.HasValue)
			{
				var classIds = DbContext.TrainingOrganizationClasses
					.Where(e => e.TrainingOrganizationManageUserId == query.TrainingOrganizationManageUserId).Select(e => e.Id);
				var studentIds = DbContext.TrainingOrganizationClassStudents.Where(e =>
					classIds.Contains(e.TrainingOrganizationClassId)).Select(e => e.GenearchChildId);
				ex = ex.And(e => studentIds.Contains(e.Id));
			}
			return ex;
		}
	}
}
