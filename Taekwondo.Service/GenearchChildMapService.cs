using System.Linq;
using Common.Data;
using Common.Models;
using Common.Extensions;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Student;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class GenearchChildMapService:BaseService
	{
	 
		private readonly GenearchChildMapRepository _genearchChildMapRepository;
 
		public GenearchChildMapService(GenearchChildMapRepository genearchChildMapRepository , DataBaseContext dbContext) : base(dbContext)
		{
            _genearchChildMapRepository = genearchChildMapRepository;
		 
		}

		/// <summary>
		/// 场馆添加家长名下的学员
		/// </summary>
		/// <param name="trainingOrganizationManageUserId">场馆管理人员Id</param>
		/// <param name="trainingOrganizationId">场馆Id</param>
		/// <param name="mobile">手机号</param>
		/// <param name="name">学员姓名</param>
		/// <param name="appellation">称谓</param>
		/// <param name="idCardNo">学员身份证号</param>
		/// <param name="classId">班级Id</param>
		public void AddGenearchMap(  long childid, long genearchid,  string appellation )
	    {
            var genearchChild = _genearchChildMapRepository.Insert(new GenearchChildMap() { Appellation = appellation, GenearchChildId = childid, GenearchId = genearchid });      
	    }
    }
}
