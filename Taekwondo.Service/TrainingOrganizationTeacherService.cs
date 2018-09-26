using Common.Data;
using Common.Extensions;
using Common.Models;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Teacher;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class TrainingOrganizationTeacherService:BaseService
	{
		private readonly TrainingOrganizationTeacherRepository _trainingOrganizationTeacherRepository;
		private readonly AccountService _accountService;
		private readonly TrainingOrganizationService _trainingOrganizationService;
        private readonly TrainingOrganizationEntService _trainingOrganizationEntService;
        public TrainingOrganizationTeacherService(TrainingOrganizationEntService trainingOrganizationEntService,TrainingOrganizationService trainingOrganizationService,AccountService accountService,TrainingOrganizationTeacherRepository trainingOrganizationTeacherRepository,DataBaseContext dbContext) : base(dbContext)
		{
			_trainingOrganizationTeacherRepository = trainingOrganizationTeacherRepository;
			_trainingOrganizationService = trainingOrganizationService;
			_accountService = accountService;
            _trainingOrganizationEntService = trainingOrganizationEntService;

        }

        /// <summary>
        /// 俱乐部添加教师
        /// </summary>
        /// <param name="trainingOrganizationManageUserId">管理员Id</param>
        /// <param name="mobile">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="name">教师的名称</param>
        public void AddTeacher(long trainingOrganizationManageUserId, string mobile, string password, string name)
	    {

		    var trainingOrganizationEnt = _trainingOrganizationEntService.GetEntByManagerId(trainingOrganizationManageUserId);
		    if (trainingOrganizationEnt==null)
		    {
			    throw new PlatformException("俱乐部管理人员才可以添加老师");
		    }
			using (var transaction = DbContext.Database.BeginTransaction())
		    {
			    var user = _accountService.AddUserAccount(mobile, password, UserType.TrainingOrganizationTeacher,name);
			    
			    _trainingOrganizationTeacherRepository.Insert(new TrainingOrganizationTeacher
			    {
				    TeacherId = user.Id,
				    TeacherName = name,
				    TrainingOrganizationId = trainingOrganizationEnt.Id,
					TrainingOrganizationManageUserId = trainingOrganizationEnt.ManagerId,
                    Mobile = mobile
			    });
			    transaction.Commit();
		    }
	    }

		/// <summary>
		/// 场馆更新教师信息
		/// </summary>
		/// <param name="trainingOrganizationManageUserId"></param>
		/// <param name="request"></param>
		public void UpdateTeacher(long trainingOrganizationManageUserId, TeacherUpdateRequest request)
		{
            var trainingOrganizationEnt = _trainingOrganizationEntService.GetEntByManagerId(trainingOrganizationManageUserId);
            if (trainingOrganizationEnt == null)
            {
                throw new PlatformException("俱乐部管理人员才可以添加老师");
            }
            //var trainingOrganization = _trainingOrganizationService.Get(request.TrainingOrganizationId);
            //if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
            //{
            //	throw new PlatformException("该场馆的管理人员才可以为该场馆添加老师");
            //}
            using (var transaction = DbContext.Database.BeginTransaction())
			{
				var trainingOrganizationTeacher = _trainingOrganizationTeacherRepository.Get(request.Id);
				var user = _accountService.GetUserAccount(trainingOrganizationTeacher.TeacherId);
				if (request.Name.IsNotNullOrEmpty())
				{
					trainingOrganizationTeacher.TeacherName = request.Name;
					user.UserName = request.Name;
				}
				if (request.Mobile.IsNotNullOrEmpty())
				{
					trainingOrganizationTeacher.Mobile = request.Mobile;
					user.Mobile = request.Mobile;
				}
				if (request.Password.IsNotNullOrEmpty())
				{
					_accountService.RestUserAccount(user.Id, request.Password);
				}
				_trainingOrganizationTeacherRepository.Update(trainingOrganizationTeacher);
				transaction.Commit();
			}
		}

		public void DeleteTeacher(long trainingOrganizationManageUserId, long teacherId)
		{
            _trainingOrganizationTeacherRepository.Delete(teacherId);
        }

		/// <summary>
	    /// 获取教师列表
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public QueryResult<TrainingOrganizationTeacher> QueryTeacher(TrainingOrganizationTeacherQuery query)
	    {
		    return _trainingOrganizationTeacherRepository.Query(query);
	    }
	}
}
