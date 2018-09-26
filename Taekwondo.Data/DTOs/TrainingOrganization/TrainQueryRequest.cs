using Common.Data;

namespace Taekwondo.Data.DTOs.TrainingOrganization
{
	/// <inheritdoc />
	/// <summary>
	/// 场馆查询
	/// </summary>
    public class TrainQueryRequest:BasePageRequest<TrainQueryResponse>
    {
		/// <summary>
		/// 场馆名称
		/// </summary>
		public string Name { set; get; }
    }

	public class TrainQueryResponse : QueryResult<Entities.TrainingOrganizationEnt>
	{

	}
}
