using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Extensions;
using Common.Models;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taekwondo.Data;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// </summary>
	[Route("api/excel/[action]")]
    public class ExcelController:BaseController
	{
		private readonly DataBaseContext _dbContext;
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="dbContext"></param>
		public ExcelController(DataBaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// 导出初始数据
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult Init()
		{
			var t1Xq = _dbContext.校区.FromSql("select next value for Ids as Id,* from [校区]").ToList();
			var t2Js = _dbContext.教师.FromSql("select next value for Ids as Id,* from [教师]").ToList();
			var t3Bj = _dbContext.班级.FromSql("select next value for Ids as Id,* from [班级]").ToList();
			var t4Jz = _dbContext.家长.FromSql("select next value for Ids as Id,* from [家长]").ToList();
			var t5Xy = _dbContext.学员.FromSql("select next value for Ids as Id,* from [学员]").ToList();

			var manageUser = _dbContext.UserAccounts.First(e => e.Mobile == "13482850230");
			var ent = _dbContext.TrainingOrganizationEnts.First(e => e.ManagerId == manageUser.Id);
			//1.新增校区
			var trainingOrganizations = new List<TrainingOrganization>();
			t1Xq.ForEach(e =>
			{
				var trainingOrganization =
					_dbContext.TrainingOrganizations.FirstOrDefault(f => f.TrainingOrganizationEntId == ent.Id&&f.TrainingOrganizationName==e.名称);
				if (trainingOrganization == null)
				{
					trainingOrganization = new TrainingOrganization
					{
						TrainingOrganizationName = e.名称,
						Address = e.地址,
						TrainingOrganizationEntId = ent.Id,
						TrainingOrganizationManagerUserId = ent.ManagerId
					};
					_dbContext.TrainingOrganizations.Add(trainingOrganization);
					_dbContext.SaveChanges();
				}
				trainingOrganizations.Add(trainingOrganization);
			});
			//2.老师账号新增
			var teacherUserAccounts = new List<UserAccount>();
			t2Js.ForEach(e =>
			{
				var teacherUserAccount = _dbContext.UserAccounts.FirstOrDefault(a => a.Mobile == e.手机号);
				if (teacherUserAccount == null)
				{
					teacherUserAccount = new UserAccount
					{
						Mobile = e.手机号,
						Password = e.初始密码,
						UserType = (int)UserType.TrainingOrganizationTeacher,
						UserName = e.名称
					};
					_dbContext.UserAccounts.Add(teacherUserAccount);
					_dbContext.SaveChanges();
				}
				teacherUserAccounts.Add(teacherUserAccount);
			});
			//3.1 新增校区班级
			var clses = new List<TrainingOrganizationClass>();
			t3Bj.ForEach(t3 =>
			{
				var org = trainingOrganizations.First(e => e.TrainingOrganizationName == t3.所属校区);
				var cls = _dbContext.TrainingOrganizationClasses.FirstOrDefault(e =>
					e.TrainingOrganizationId == org.Id && e.ClassName == t3.名称);
				if (cls == null)
				{
					cls = new TrainingOrganizationClass
					{
						TrainingOrganizationId = org.Id,
						ClassName = t3.名称,
						Dan = (DanType)t3.段位.GetValueByDescription<DanType>(),
						TrainingOrganizationManageUserId = org.TrainingOrganizationManagerUserId
					};
					_dbContext.TrainingOrganizationClasses.Add(cls);
					_dbContext.SaveChanges();
				}
				cls.OrgName = org.TrainingOrganizationName;
				clses.Add(cls);
				//3.2 同步班级教师
				var teacherNames = t3.任课老师.Split("，");
				var teacherAccounts = teacherUserAccounts.Where(e => teacherNames.Contains(e.UserName)).ToList();
				teacherAccounts.ForEach(teacherAccount =>
				{
					var teacher = _dbContext.TrainingOrganizationTeachers.FirstOrDefault(e =>
						e.TrainingOrganizationId == cls.TrainingOrganizationId && e.Mobile == teacherAccount.Mobile);
					if (teacher == null)
					{
						teacher = new TrainingOrganizationTeacher
						{
							Mobile = teacherAccount.Mobile,
							TeacherName = teacherAccount.UserName,
							TrainingOrganizationId = cls.TrainingOrganizationId,
							TrainingOrganizationManageUserId = cls.TrainingOrganizationManageUserId,
							TeacherId = teacherAccount.Id
						};
						_dbContext.TrainingOrganizationTeachers.Add(teacher);
						_dbContext.SaveChanges();
					}
					var teacherMap = _dbContext.TrainingOrganizationClassTeacherMaps.FirstOrDefault(e =>
						e.TrainingOrganizationClassId == cls.Id && e.TrainingOrganizationTeacherId == teacher.TeacherId);
					if (teacherMap == null)
					{
						teacherMap = new TrainingOrganizationClassTeacherMap
						{
							TeacherInClassType = TeacherType.Headmaster,
							TrainingOrganizationClassId = cls.Id,
							TrainingOrganizationTeacherId = teacher.TeacherId
						};
						_dbContext.TrainingOrganizationClassTeacherMaps.Add(teacherMap);
						_dbContext.SaveChanges();
					}
				});
				var lessonList = InitLesson(cls.Id, t3.上课时间段);
				_dbContext.TrainingOrganizationClassLessons.AddRange(lessonList);
				_dbContext.SaveChanges();
			});
			//4.1 插入学员
			t5Xy.ForEach(t5 =>
			{
				var cls = clses.First(e => e.ClassName == t5.班级 && e.OrgName == t5.校区);
				var dan = (DanType) t5.段位.Replace("带","").GetValueByDescription<DanType>();
				var student = new GenearchChild {GenearchChildName = t5.姓名, Dan = dan};
				_dbContext.GenearchChildren.Add(student);
				_dbContext.SaveChanges();
				var ps = t4Jz.Where(e => e.校区 == t5.校区 && t5.班级 == e.班级 && e.学员姓名.Split("，").Contains(t5.姓名)).ToList();
				ps.ForEach(p =>
				{
					var pUserAccount = _dbContext.UserAccounts.FirstOrDefault(a => a.Mobile == p.手机号);
					if (pUserAccount == null)
					{
						pUserAccount = new UserAccount
						{
							Mobile = p.手机号,
							Password = p.初始密码,
							UserType = (int)UserType.TrainingOrganizationGenearch,
							UserName = p.姓名
						};
						_dbContext.UserAccounts.Add(pUserAccount);
						_dbContext.SaveChanges();
					}
					var genearchChildMap =
						_dbContext.GenearchChildMaps.FirstOrDefault(e =>
							e.GenearchChildId == student.Id && e.GenearchId == pUserAccount.Id);
					if (genearchChildMap == null)
					{
						genearchChildMap = new GenearchChildMap
						{
							GenearchChildId = student.Id,
							Appellation = p.称谓,
							GenearchId = pUserAccount.Id
						};
						_dbContext.GenearchChildMaps.Add(genearchChildMap);
						_dbContext.SaveChanges();
					}
				});
				var clsStudent = _dbContext.TrainingOrganizationClassStudents.FirstOrDefault(e =>
					e.GenearchChildId == student.Id && e.TrainingOrganizationClassId == cls.Id);
				if (clsStudent == null)
				{
					clsStudent = new TrainingOrganizationClassStudent
					{
						GenearchChildId = student.Id,
						TrainingOrganizationClassId = cls.Id,
						TrainingOrganizationId = cls.TrainingOrganizationId,
						TrainingOrganizationManageUserId = cls.TrainingOrganizationManageUserId,
						TrainingOrganizationClassStudentStatus = (int)TrainingOrganizationClassStudentStatus.Start
					};
					_dbContext.TrainingOrganizationClassStudents.Add(clsStudent);
					_dbContext.SaveChanges();
				}
				var lessonCount = t5.剩余课时.To(0);
				var lessons = _dbContext.TrainingOrganizationClassLessons.Where(e =>
					e.StartTime > DateTime.Now && e.TrainingOrganizationClassId == cls.Id).OrderBy(e=>e.StartTime).Take(lessonCount).ToList();
				lessons.ForEach(lesson =>
				{
					var map = _dbContext.TrainingOrganizationClassStudentLessonMaps.FirstOrDefault(e =>
						e.GenearchChildId == student.Id && e.TrainingOrganizationClassLessonId == lesson.Id);
					if (map == null)
					{
						map = new TrainingOrganizationClassStudentLessonMap
						{
							GenearchChildId = student.Id,
							TrainingOrganizationClassLessonId = lesson.Id
						};
						_dbContext.TrainingOrganizationClassStudentLessonMaps.Add(map);
						_dbContext.SaveChanges();
					}
				});
			});
			return this.Success();
		}

		private List<TrainingOrganizationClassLesson> InitLesson(long clsId, string time)
		{
			var list = new List<TrainingOrganizationClassLesson>();
			var dayOfWeekStr = time.Substring(0, 2);
			var startTime = time.Substring(2, time.Length - 2).Split("-")[0];
			var endTime = time.Substring(2, time.Length - 2).Split("-")[1];
			var dayOfWeek = CtorDayOfWeek(dayOfWeekStr);
			for (int i = 0; i < 7000; i++)
			{
				var day = DateTime.Now.AddDays(i);
				if (day.DayOfWeek == dayOfWeek)
				{
					var lesson = new TrainingOrganizationClassLesson
					{
						TrainingOrganizationClassId = clsId,
						StartTime = CtorTime(day, startTime),
						EndTime = CtorTime(day, endTime)
					};
					list.Add(lesson);
				}
			}
			return list;
		}

		private DayOfWeek CtorDayOfWeek(string dayOfWeekStr)
		{
			switch (dayOfWeekStr)
			{
				case "周日":
					return DayOfWeek.Sunday;
				case "周一":
					return DayOfWeek.Monday;
				case "周二":
					return DayOfWeek.Tuesday;
				case "周三":
					return DayOfWeek.Wednesday;
				case "周四":
					return DayOfWeek.Thursday;
				case "周五":
					return DayOfWeek.Friday;
				case "周六":
					return DayOfWeek.Saturday;
			}
			throw new PlatformException("错误的时间");
		}

		private  DateTime CtorTime(DateTime day, string time)
		{
			return new DateTime(day.Year, day.Month, day.Day, time.Split(":")[0].To(0), time.Split(":")[1].To(0), 0);
		}
	}
}
