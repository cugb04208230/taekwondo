using System;
using System.Collections.Generic;
using Common.Data;
using Taekwondo.Data.Entities;

namespace Taekwondo.Data.DTOs.Homework
{
	/// <inheritdoc />
	/// <summary>
	/// 作业列表查询
	/// </summary>
    public class HomeworkQueryRequest:BasePageRequest<HomeworkQueryResponse>
    {
		/// <summary>
		/// 老师查询时选填
		/// </summary>
		public long? ClassId { set; get; }

		/// <summary>
		/// 家长查询时必填
		/// </summary>
		public long? StudentId { set; get; }
    }

	/// <summary>
	/// 作业查询结果
	/// </summary>
	public class HomeworkQueryResponse:QueryResult<TrainingOrganizationClassHomework>
	{
	}

	
	
}
