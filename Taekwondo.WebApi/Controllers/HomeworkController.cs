using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using Taekwondo.Data.DTOs.Homework;
using Taekwondo.Data.DTOs.Teacher;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;
using Taekwondo.Service;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 家庭作业
	/// </summary>
	[Route("api/[controller]/[action]")]
	[Auth]
	public class HomeworkController:BaseController
	{
		private readonly TrainingOrganizationClassHomeworkService _homeworkService;
		private readonly TrainingOrganizationClassHomeworkAnswerService _answerService;
		private readonly TrainingOrganizationTeacherService _teacherService;

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="answerService"></param>
		/// <param name="homeworkService"></param>
		/// <param name="teacherService"></param>
		public HomeworkController(TrainingOrganizationTeacherService teacherService,TrainingOrganizationClassHomeworkAnswerService answerService,TrainingOrganizationClassHomeworkService homeworkService)
		{
			_homeworkService = homeworkService;
			_answerService = answerService;
			_teacherService = teacherService;
		}

		/// <summary>
		/// 获取作业列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Query(HomeworkQueryRequest request)
		{
			var user = UserAccount;
			var query = new TrainingOrganizationClassHomeworkQuery
			{
				PageIndex = request.PageIndex,
				PageSize = request.PageSize
			};
			if (user.UserType == (int) UserType.TrainingOrganizationTeacher)//老师
			{
				if ((request.ClassId ?? 0) == 0)
				{
					throw new PlatformException("班级信息不能为空");
				}
				query.TrainingOrganizationClassId = new []{ request.ClassId??0 };
				query.TrainingOrganizationClassTeacherId = user.Id;
			}
			else if (user.UserType == (int) UserType.TrainingOrganizationGenearch)//家长
			{
				if ((request.StudentId ?? 0) == 0)
				{
					throw new PlatformException("学员信息不能为空");
				}
				query.GenearchChildId = request.StudentId;
			}
			var queryResult = _homeworkService.Query(query);
			var list = new List<HomeworkDto>();
			var answers = _answerService.Query(new TrainingOrganizationClassHomeworkAnswerQuery
			{
				GenearchChildId = request.StudentId,
				HomeworkIds = queryResult.List.Select(e => e.Id).ToArray(),
				PageSize = int.MaxValue
			});
			foreach (var trainingOrganizationClassHomework in queryResult.List)
			{
				trainingOrganizationClassHomework.Answers = answers.List
					.Where(e => e.TrainingOrganizationClassHomeworkId == trainingOrganizationClassHomework.Id).ToList();
			}
			foreach (var item in queryResult.List)
			{
				var dto = new HomeworkDto
				{
					CreatedAt = item.CreatedAt,
					Files = item.Files.IsNullOrEmpty() ? new List<string>() : item.Files?.Split(",").Select(GetFileNetPath).ToList(),
					Images = item.Images.IsNullOrEmpty() ? new List<string>() : item.Images?.Split(",").Select(GetImageNetPath).ToList(),
					Id =item.Id,
					Summary = item.Summary,
					Title = item.Title,
					Answers = new List<HomeworkAnswerDto>(),
					Teacher = new TeacherDto{TeacherId = item.TrainingOrganizationClassTeacherId,HeadPic = GetHeadPic(item.TrainingOrganizationClassTeacherId)}
				};
				if (item.Answers != null)
				{
					foreach (var answer in item.Answers)
					{
						var answerDto = new HomeworkAnswerDto
						{
							Id = answer.Id,
							CreatedAt = answer.CreatedAt,
							Files = answer.Files.IsNullOrEmpty() ? new List<string>() : answer.Files?.Split(",").Select(GetFileNetPath).ToList(),
							Images = answer.Images.IsNullOrEmpty() ? new List<string>() : answer.Images?.Split(",").Select(GetImageNetPath).ToList(),
							HomeworkId = answer.TrainingOrganizationClassHomeworkId,
							Readovered = answer.Readovered,
							ReadoverText = answer.ReadoverText,
							Stars = answer.Stars,
							StudentId = answer.GenearchChildId,
							StudentName = answer.GenearchChildName,
							Summary = answer.Summary,
							HeadPic = GetHeadPic(answer.GenearchId)
						};
						dto.Answers.Add(answerDto);
					}
				}
				list.Add(dto);
			}
			list.ForEach(e =>
			{
				var answer = answers.List.FirstOrDefault(a => a.TrainingOrganizationClassHomeworkId == e.Id&&a.GenearchChildId==request.StudentId);
				if (answer == null)
				{
					e.Status = (int)TrainingOrganizationClassHomeworkAnswerStatus.NotSub;
				}
				else
				{
					e.Status = answer.Readovered
						? (int)TrainingOrganizationClassHomeworkAnswerStatus.Marked
						: (int)TrainingOrganizationClassHomeworkAnswerStatus.Subed;
					e.Stars = answer.Stars;
				}
			});
			var teachers = _teacherService.QueryTeacher(new TrainingOrganizationTeacherQuery { TeacherIds = list.Select(e => e.Teacher.TeacherId).ToArray() });
			list.ForEach(e =>
			{
				var teacher = teachers.List.FirstOrDefault(t => t.TeacherId == e.Teacher.TeacherId);
				if (teacher != null)
				{
					e.Teacher.TeacherName = teacher.TeacherName;
				}
			});
			return this.Success(list);
		}

		/// <summary>
		/// 答题
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Answer(AnswerRequest request)
		{
			var allFileNames = SaveFiles();
			var allFiles = allFileNames.Split(",");
			var files = request.Files.IsNullOrEmpty() ? allFiles.Where(e => e.EndsWith("mp4") || e.EndsWith("mov")).Join(",") : request.Files;
			var images = request.Images.IsNullOrEmpty() ? allFiles.Where(e => e.EndsWith("jpg") || e.EndsWith("jpeg")).Join(",") : request.Images;
			var user = UserAccount;
			_answerService.Answer(user.Id,request.StudentId,request.HomeworkId,request.Summary,files, images);
			return this.Success();
		}

		/// <summary>
		/// 发布家庭作业
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult PublishHomework(PublishHomeworkRequest request)
		{
			var allFileNames = SaveFiles();
			LogManager.GetLogger("FilesName").Info(allFileNames);
			var allFiles = allFileNames.Split(",").ToList();
			LogManager.GetLogger("FilesName").Info(allFiles.Count);
			var files = request.Files.IsNullOrEmpty()? allFiles.Where(e=>e.EndsWith("mp4")|| e.EndsWith("mov")).Join(","): request.Files;
			var images = request.Images.IsNullOrEmpty() ? allFiles.Where(e => e.EndsWith("jpg")|| e.EndsWith("jpeg")).Join(","):request.Images;
			_homeworkService.PublishHomework(UserAccountId??0, request.ClassId, request.Title, request.Summary, files, images);
			return this.Success();
		}

		/// <summary>
		/// 作业详情以及答题情况查询
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult HomeworkDetail(HomeworkDetailRequest request)
		{
			var user = UserAccount;
			var homework = _homeworkService.Detail(request.HomeworkId, request.StudentId);
			request.ResponseModel = new HomeworkDetailResponse
			{
				Homework = new HomeworkDto
				{
					CreatedAt = homework.CreatedAt,
					Id = homework.Id,
					Summary = homework.Summary,
					Title = homework.Title,
					Files = homework.Files.IsNullOrEmpty()?new List<string>():homework.Files?.Split(",").Select(GetFileNetPath).ToList(),
					Images = homework.Images.IsNullOrEmpty() ? new List<string>() : homework.Images?.Split(",").Select(GetImageNetPath).ToList(),
					Answers = homework.Answers.Select(e => new HomeworkAnswerDto
					{
						Id = e.Id,
						CreatedAt = e.CreatedAt,
						HomeworkId = e.TrainingOrganizationClassHomeworkId,
						StudentId = e.GenearchChildId,
						StudentName = e.GenearchChildName,
						Summary = e.Summary,
						Files = e.Files.IsNullOrEmpty()?new List<string>():e.Files?.Split(",").Select(GetFileNetPath).ToList(),
						Images = e.Images.IsNullOrEmpty()?new List<string>():e.Images?.Split(",").Select(GetImageNetPath).ToList(),
						Readovered = e.Readovered,
						ReadoverText = e.ReadoverText,
						Stars = e.Stars,
						HeadPic = GetHeadPic(e.GenearchId)
					}).ToList()
				},
				CanAnswer = homework.Answers.Any(e=>e.GenearchChildId==(request.StudentId??0))
			};

			var answers = _answerService.Query(new TrainingOrganizationClassHomeworkAnswerQuery
			{
				GenearchChildId = request.StudentId,
				HomeworkIds = new[] { homework.Id }
			});
			var answer = answers.List.FirstOrDefault(a => a.TrainingOrganizationClassHomeworkId == homework.Id && a.GenearchChildId == request.StudentId);
			if (answer == null)
			{
				request.ResponseModel.Homework.Status = (int)TrainingOrganizationClassHomeworkAnswerStatus.NotSub;
			}
			else
			{
				request.ResponseModel.Homework.Status = answer.Readovered
					? (int)TrainingOrganizationClassHomeworkAnswerStatus.Marked
					: (int)TrainingOrganizationClassHomeworkAnswerStatus.Subed;
			}
			return this.Success(request.ResponseModel.Homework);
		}

		/// <summary>
		/// 作业批阅
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Marking(MarkingRequest request)
		{
			_answerService.Marking(UserAccount.Id,request.AnswerId,request.ReadoverText,(request.Stars ?? 0)==0?5: request.Stars.Value);
			return this.Success();
		}

	}
}
