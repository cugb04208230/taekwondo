using Common.Data;

namespace Taekwondo.Data.DTOs.TrainingOrganization
{
	/// <inheritdoc />
	/// <summary>
	/// 训练场馆查询
	/// </summary>
    public class TrainingOrganizationQueryRequest:BasePageRequest<TrainingOrganizationQueryResponse>
	{
		/// <summary>
		/// 场馆名称，支持模糊查询
		/// </summary>
		public string Name { set; get; }
	}
	/// <summary>
	/// 训练场馆查询结果
	/// </summary>
	public class TrainingOrganizationQueryResponse:QueryResult<Entities.TrainingOrganization>
	{
	}
}
