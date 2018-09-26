using System;
using Common.Data;
using Common.Extensions;
using Common.Enums;
using Common.Models;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Genearch;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class GenearchService:BaseService
	{
		private readonly GenearchRepository _genearchRepository;
        private readonly TrainingOrganizationService _trainingOrganizationService;
        private readonly AccountService _accountService;
		public GenearchService(AccountService accountService, GenearchRepository genearchRepository, TrainingOrganizationService trainingOrganizationService, DataBaseContext dbContext) : base(dbContext)
		{
			_genearchRepository = genearchRepository;
			_accountService = accountService;
            _trainingOrganizationService = trainingOrganizationService;

        }

	    /// <summary>
	    /// 场馆添加家长
	    /// </summary>
	    /// <param name="trainingOrganizationId">场馆Id</param>
	    /// <param name="mobile">手机号</param>
	    /// <param name="password">密码</param>
	    /// <param name="name">家长名称</param>
	    /// <param name="idCardNo">家长身份证号</param>
	    /// <param name="gender">家长性别</param>
	    /// <param name="birthday">家长生日</param>
	    /// <param name="address">家长联系地址</param>
	    public void AddGenearch(long trainingOrganizationId, GenearchAddRequest request)
	    {
		    using (var transaction = DbContext.Database.BeginTransaction())
		    {
			    var user = _accountService.AddUserAccount(request.Mobile, request.Password, UserType.TrainingOrganizationGenearch, request.GenearchName);
			    _genearchRepository.Insert(new Genearch
			    {
				    Id = user.Id,
				    GenearchName = request.GenearchName,
				    IdCardNo = request.IdCardNo,
				    Gender = request.Gender,
				    Birthday = request.Birthday,
				    Mobile = request.Mobile,
				    Address = request.Address
			    });
			    transaction.Commit();
		    }
	    }

	    /// <summary>
	    /// 获取家长列表
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public QueryResult<Genearch> QueryGenearch(GenearchQuery query)
	    {
		    return _genearchRepository.Query(query);
	    }


        public void DeleteGenearch(long trainingOrganizationManageUserId, long gId)
        {
            var cls = _genearchRepository.Get(gId);
            if (cls == null)
            {
                throw new PlatformException("错误的家长编号");
            }
            //if (cls.TrainingOrganizationManageUserId != trainingOrganizationManageUserId)
            //{
            //    throw new PlatformException("管理员才可以删除班级哦");
            //}
            _genearchRepository.Delete(cls);
        }

        /// <summary>
		/// 场馆更新家长信息
		/// </summary>
		/// <param name="trainingOrganizationManageUserId"></param>
		/// <param name="request"></param>
		public void UpdateGenearch(long trainingOrganizationManageUserId, GenearchUpdateRequest request)
        {
            var trainingOrganization = _trainingOrganizationService.Get(trainingOrganizationManageUserId);
            if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
            {
                throw new PlatformException("该场馆的管理人员才可以为该场馆添加学员");
            }
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                var genearch = _genearchRepository.Get(request.Id);
                if (request.GenearchName.IsNotNullOrEmpty())
                {
                    genearch.GenearchName = request.GenearchName;
                }
                if (request.IdCardNo.IsNotNullOrEmpty())
                {
                    genearch.IdCardNo = request.IdCardNo;
                }
                if (request.Mobile.IsNotNullOrEmpty())
                {
                    genearch.Mobile = request.Mobile;
                }
                if (request.Gender!=0)
                {
                    genearch.Gender = request.Gender;
                }
                if (request.Birthday!=null)
                {
                    genearch.Birthday = request.Birthday;

                }
                if (request.Address.IsNotNullOrEmpty())
                {
                    genearch.Address = request.Address;
                }
                _genearchRepository.Update(genearch);
                transaction.Commit();
            }
        }

    }
}
