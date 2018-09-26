using System;
using Common.Data;
using Common.Extensions;
using Common.Models;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Account;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
	/// <summary>
	/// 账号服务
	/// </summary>
    public class AccountService : BaseService
	{
		private readonly UserAccountRepository _userAccountRepository;
		private readonly LoginLogRepository _loginLogRepository;
		private readonly VerifyCodeRepository _verifyCodeRepository;
		public AccountService(VerifyCodeRepository verifyCodeRepository,DataBaseContext dbContext,UserAccountRepository userAccountRepository,LoginLogRepository loginLogRepository):base(dbContext)
		{
			_userAccountRepository = userAccountRepository;
			_loginLogRepository = loginLogRepository;
			_verifyCodeRepository = verifyCodeRepository;
		}

		public UserAccount AddUserAccount(string mobile, string password, UserType userType,string name,string headPic="headpic.jpg")
		{
			if (mobile.IsNullOrEmpty()||password.IsNullOrEmpty())
			{
				throw new PlatformException(ErrorCode.ErrorMobileOrPassword);
			}
			var user = _userAccountRepository.FirstOrDefault(e => e.Mobile == mobile);
			if (user!=null)
			{
				return user;
			}
			var userAccount = _userAccountRepository.Insert(new UserAccount
			{
				Mobile = mobile,
				Password = password??"111111",
				Status = 0,
				UserType = (int)userType,
				UserName = name,
				HeadPic = headPic
			});
			return userAccount;
		}

		public UserAccount GetUserAccount(long id)
		{
			return _userAccountRepository.FirstOrDefault(e => e.Id == id);
		}


		public void RestUserAccount(long id,string psd)
		{
			var user =  _userAccountRepository.FirstOrDefault(e => e.Id == id);
			user.Password = psd;
			_userAccountRepository.Update(user);
		}

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ip"></param>
		/// <returns></returns>
		public RegisterResponse Register(RegisterRequest request, string ip = "")
		{
			var account = _userAccountRepository.FirstOrDefault(e => e.Mobile == request.Mobile);
			if (account != null)
			{
				throw new PlatformException(ErrorCode.RegisterMobileIsExisted);
			}
			var verifyCode = _verifyCodeRepository.LastOrDefault(e =>
				e.Mobile == request.Mobile && e.Code == request.Code && e.ExpriedAt >= DateTime.Now&&e.Type==(int)SmsType.Register);
			if (verifyCode == null)
			{
				throw new PlatformException(ErrorCode.ErrorVerifycode);
			}
			var userAccount = AddUserAccount(request.Mobile, request.Password, UserType.TrainingOrganizationGenearch,"");
			request.ResponseModel=new RegisterResponse
			{
				UserInfo = new UserInfo { Type = userAccount.UserType },
				Token = GetToken(userAccount, ip)
			};
			return request.ResponseModel;
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ip"></param>
		/// <returns></returns>
		public LoginResponse Login(LoginRequest request,string ip="")
		{
			var account =
				_userAccountRepository.FirstOrDefault(e => e.Mobile == request.Mobile && e.Password == request.Password);
			if (account == null)
			{
				throw new PlatformException(ErrorCode.ErrorMobileOrPassword);
			}
			if (account.Status == (int) UserAccountEnum.Stop)
			{
				throw new PlatformException(ErrorCode.AccountIsStop);
			}
			request.ResponseModel = new LoginResponse
			{
				UserInfo = new UserInfo{Type = account.UserType,UserId = account.Id,UserName = account.UserName},
				Token = GetToken(account,ip)
			};
			return request.ResponseModel;
		}

		/// <summary>
		/// 重置密码
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ip"></param>
		/// <returns></returns>
		public ResetPasswordResponse ResetPassword(ResetPasswordRequest request, string ip = "")
		{
			var account =
				_userAccountRepository.FirstOrDefault(e => e.Mobile == request.Mobile);
			if (account == null)
			{
				throw new PlatformException(ErrorCode.MobileIsNotExisted);
			}
			var verifyCode = _verifyCodeRepository.LastOrDefault(e =>
				e.Mobile == request.Mobile && e.Code == request.Code && e.ExpriedAt >= DateTime.Now && e.Type == (int)SmsType.ResetPassword);
			if (verifyCode == null)
			{
				throw new PlatformException(ErrorCode.ErrorVerifycode);
			}
			account.Password = request.Password;
			_userAccountRepository.Update(account);
			request.ResponseModel = new ResetPasswordResponse
			{
				UserInfo = new UserInfo { Type = account.UserType },
				Token = GetToken(account, ip)
			};
			return request.ResponseModel;
		}

		/// <summary>
		/// 检查Token的有效性
		/// </summary>
		/// <param name="token">登录票据</param>
		/// <returns></returns>
		public UserAccount CheckToken(string token)
		{
			var loginLog = _loginLogRepository.FirstOrDefault(e => e.Token == token);
			if (loginLog == null||loginLog.ExpiredAt<DateTime.Now)
			{
				throw new PlatformException(ErrorCode.InvalidToken);
			}
			return _userAccountRepository.Get(loginLog.UserId);
		}

		#region

		/// <summary>
		/// 获取授权票据
		/// </summary>
		/// <param name="account">账号信息</param>
		/// <param name="ip">登录Ip</param>
		/// <returns></returns>
		private string GetToken(UserAccount account, string ip = "")
		{
			//Todo 暂不做缓存
			var token = Guid.NewGuid().ToString("N");
			_loginLogRepository.Insert(new LoginLog
			{
				UserId = account.Id,
				Token = token,
				ExpiredAt = DateTime.Now.Date.AddMonths(1),
				Ip = ip
			});
			return token;
		}

		#endregion
	}
}
