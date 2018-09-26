using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Common.Data;
using Common.Di;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.SystemSetting;
using Taekwondo.Data.Entities;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 配置
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class SettingsController: BaseController
	{
		private readonly DataBaseContext _dbContext;
		/// <summary>
		/// 配置
		/// </summary>
		/// <param name="dbContext"></param>
		public SettingsController(DataBaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// 更新信息接口
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult UpdateSettings(string code,string value)
		{
			var setting = _dbContext.SystemSettings.FirstOrDefault(e => e.Code == code);
			if (setting == null)
			{
				setting = new SystemSetting {Code = code,Value = value};
				_dbContext.SystemSettings.Add(setting);
			}
			else
			{
				setting.Value = value;
			}
			_dbContext.SaveChanges();
			return this.Success();
		}
		/// <summary>
		/// 更新信息接口
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult UpdateInfo(UpdateInfoRequest request)
		{
			var appEditionIosSetting = _dbContext.SystemSettings.FirstOrDefault(e => e.Code == SystemSettingCode.AppEditionIos);
			var appEditionAndroidSetting = _dbContext.SystemSettings.FirstOrDefault(e => e.Code == SystemSettingCode.AppEditionAndroid);
			var appIosIsMandatoryUpdateSetting = _dbContext.SystemSettings.FirstOrDefault(e => e.Code == SystemSettingCode.AppIosIsMandatoryUpdate);
			var appAndroidIsMandatoryUpdateSetting = _dbContext.SystemSettings.FirstOrDefault(e => e.Code == SystemSettingCode.AppAndroidIsMandatoryUpdate);
			request.ResponseModel = new UpdateInfoResponse
			{
				Path = $"{Settings.ApiHost}apk/{appEditionAndroidSetting?.Value}",
				AppEditionIos = appEditionIosSetting?.Value,
				AppIosIsMandatoryUpdate = appIosIsMandatoryUpdateSetting?.Value?.To<bool>()??false,
				AppEditionAndroid = appEditionAndroidSetting?.Value,
				AppAndroidIsMandatoryUpdate = appAndroidIsMandatoryUpdateSetting?.Value?.To<bool>() ?? false
			};
			return this.Success(request.ResponseModel);
		}


		#region AppEdition

		/// <summary>
		/// 版本替换
		/// </summary>
		/// <param name="os"></param>
		/// <param name="edition"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult AppEdition(string os, string edition)
		{
			if (os != "ios" && os != "android")
			{
				return this.Fail("错误的操作系统");
			}
			Regex reg = new Regex(@"^[\d]+\.[\d]+\.[\d]+$");
			var match = reg.Match(edition);
			if (!match.Success)
			{
				return this.Fail("错误的版本号");
			}
			var dto = new AppEditionDto
			{
				Edition = "",
				Path = "",
				IsMandatory = false
			};
			var lastAppEdition = _dbContext.AppEditions.Where(e=> e.Os == os).OrderByDescending(e => e.Id).FirstOrDefault();
			if (lastAppEdition == null)
			{
				return this.Success(dto);
			}
			var rst = CompareAppEdition(edition, lastAppEdition.Edition);
			switch (rst)
			{
				case 1:
				case 3:
					dto.IsUpdate = false;
					break;
				case 2:
					var appEdition = _dbContext.AppEditions.FirstOrDefault(e => e.Edition == edition && e.Os == os);
					var queryable = _dbContext.AppEditions.Where(e => e.Os == os);
					if (appEdition != null)
					{
						queryable = queryable.Where(e => e.CreatedAt >= appEdition.CreatedAt);
					}
					var last = queryable.OrderByDescending(e => e.CreatedAt).First();
					dto.Edition = last.Edition;
					dto.Content = last.Content;
					dto.Path = last.Path;
					dto.IsMandatory = queryable.Where(e=>e.Edition!=edition).Any(e => e.IsMandatory);
					dto.IsUpdate = true;
					break;
			}
			return this.Success(dto);
		}

		/// <summary>
		/// 比较,本地版本和传递版本
		/// </summary>
		/// <param name="input">传递参数</param>
		/// <param name="local">服务端最新版本</param>
		/// <returns>
		/// 1.input 版本号大于local
		/// 2.input 版本号小于local
		/// 3.input 版本号等于local
		/// </returns>
		private int CompareAppEdition(string input, string local)
		{
			var inputItems = input.Split(".");
			var localItems = local.Split(".");
			for (int i = 0; i < 3; i++)
			{
				var num1 = int.Parse(inputItems[i]);
				var num2 = int.Parse(localItems[i]);
				if (num1 > num2)
				{
					return 1;
				}
				if (num1 < num2)
				{
					return 2;
				}
			}
			return 3;
		}

		#endregion

		/// <summary>
		/// InitStudentClass
		/// </summary>
		/// <param name="os"></param>
		/// <param name="edition"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult InitStudentClass()
		{
			var classIds = new long[]
			{
				100000013182,
				100000013185,
				100000013633,
				100000015553,
				100000015556
			};
			var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>();
			var classes = dbContext.Set<TrainingOrganizationClass>().Where(e => classIds.Contains(e.Id));
			var lessones = dbContext.Set<TrainingOrganizationClassLesson>()
				.Where(e => classIds.Contains(e.TrainingOrganizationClassId));
			var students = dbContext.Set<TrainingOrganizationClassStudent>()
				.Where(e => classIds.Contains(e.TrainingOrganizationClassId));
			foreach (var classId in classIds)
			{
				var itemLessones = lessones.Where(e => e.TrainingOrganizationClassId == classId).OrderBy(e => e.StartTime).Take(10);
				var studentIds = students.Where(e => e.TrainingOrganizationClassId == classId).Select(e => e.GenearchChildId).ToList();
				studentIds.ForEach(studentId =>
				{
					var studentLessons = itemLessones.Select(e => new TrainingOrganizationClassStudentLessonMap
					{
						CreatedAt = DateTime.Now,
						GenearchChildId = studentId,
						LastModifiedAt = DateTime.Now,
						TrainingOrganizationClassLessonId = e.Id
					});
					dbContext.Set<TrainingOrganizationClassStudentLessonMap>().AddRange(studentLessons);
					dbContext.SaveChanges();
				});
			}
			return this.Success();
		}

	}

	public class AppEditionDto
	{

		/// <summary>
		/// 版本号
		/// </summary>
		public string Edition { get; set; }

		/// <summary>
		/// 更新地址
		/// </summary>
		public string Path { set; get; }
		/// <summary>
		/// 版本更新内容
		/// </summary>
		public string Content { set; get; }

		/// <summary>
		/// 是否为新版本
		/// </summary>
		public bool IsUpdate { set; get; }

		/// <summary>
		/// 是否需要更新
		/// </summary>
		public bool IsMandatory { set; get; }
	}
}
