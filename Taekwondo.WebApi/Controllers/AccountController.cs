using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Di;
using Common.Push.JiGuang;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Account;
using Taekwondo.Data.DTOs.Class;
using Taekwondo.Data.DTOs.Student;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 账号管理
	/// </summary>
	[Route("api/[controller]")]
	public class AccountController:BaseController
	{
		private readonly AccountService _accountService;
		private readonly TrainingOrganizationClassService _trainingOrganizationClassService;
		private readonly GenearchChildService _genearchChildService;
		private readonly TrainingOrganizationTeacherService _teacherService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="teacherService"></param>
		/// <param name="trainingOrganizationClassService"></param>
		/// <param name="genearchChildService"></param>
		/// <param name="accountService"></param>
		public AccountController(TrainingOrganizationTeacherService teacherService,TrainingOrganizationClassService trainingOrganizationClassService, GenearchChildService genearchChildService, AccountService accountService)
		{
			_accountService = accountService;
			_trainingOrganizationClassService = trainingOrganizationClassService;
			_genearchChildService = genearchChildService;
			_teacherService = teacherService;
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Login")]
	    public IActionResult Login(LoginRequest request)
	    {
		    using (var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>())
		    {
			    using (var trans = dbContext.Database.BeginTransaction())
				{
					var user = dbContext.UserAccounts.First(e => e.Mobile == "14500000001");
					user.Password = "222222";
					dbContext.Update(user);
					dbContext.SaveChanges();
					trans.Commit();
				}
		    }

		    using (var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>())
			{
				var user = dbContext.UserAccounts.First(e => e.Mobile == "14500000001");
				user.Password = "222222";
				EntityEntry<UserAccount> entry = dbContext.Entry(user);
				entry.State = EntityState.Modified;
				dbContext.SaveChanges();
			}
			Validate(request);
		    var  response = _accountService.Login(request);
		    if (response.UserInfo.Type == (int) UserType.TrainingOrganizationTeacher)
		    {
				var classes = _trainingOrganizationClassService.Query(new TrainingOrganizationClassQuery
			    {
				    TrainingOrganizationTeacherId = response.UserInfo.UserId,
				    PageSize = int.MaxValue
			    });
				response.UserInfo.ExtendData=new List<object>();
			    classes.List.ToList().ForEach(e => response.UserInfo.ExtendData.Add(ModelMapUtil.AutoMap(e, new ClassDto())));
		    }
		    if (response.UserInfo.Type == (int)UserType.TrainingOrganizationGenearch)
			{
				var students = _genearchChildService.Query(new GenearchChildQuery
			    {
				    GenearchId = response.UserInfo.UserId,
				    PageSize = int.MaxValue
			    });
			    response.UserInfo.ExtendData = new List<object>();
			    students.List.ToList().ForEach(e => response.UserInfo.ExtendData.Add(ModelMapUtil.AutoMap(e, new StudentDto())));
			}
		    response.UserInfo.HeadPic = GetHeadPic(response.UserInfo.UserId);

			return this.Success(response);
	    }


		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Register")]
		public IActionResult Register(RegisterRequest request)
		{
			Validate(request);
			var response = _accountService.Register(request);
			return this.Success(response);
		}


		/// <summary>
		/// 找回密码
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("ResetPassword")]
		public IActionResult ResetPassword(ResetPasswordRequest request)
		{
			Validate(request);
			var response = _accountService.ResetPassword(request);
			return this.Success(response);
		}


		/// <summary>
		/// 更新头像
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Auth]
		[Route("UpdateHeadPic")]
		public IActionResult UpdateHeadPic()
		{
			var accountId = UserAccountId ?? 1;
			SaveHeadPic(accountId);
			return this.Success(GetHeadPic(accountId));
		}
	}
}
