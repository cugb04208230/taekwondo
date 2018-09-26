using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Di;
using Common.Models;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Class;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class TrainingOrganizationClassService:BaseService
	{
		private readonly TrainingOrganizationTeacherRepository _trainingOrganizationTeacherRepository;
		private readonly TrainingOrganizationClassRepository _trainingOrganizationClassRepository;
		private readonly TrainingOrganizationService _trainingOrganizationService;
		private readonly TrainingOrganizationClassTeacherMapRepository _trainingOrganizationClassTeacherMapRepository;
		private readonly TrainingOrganizationClassLessonRepository _trainingOrganizationClassLessonRepository;
		public TrainingOrganizationClassService(TrainingOrganizationClassLessonRepository trainingOrganizationClassLessonRepository,TrainingOrganizationClassTeacherMapRepository trainingOrganizationClassTeacherMapRepository,TrainingOrganizationService trainingOrganizationService, TrainingOrganizationClassRepository trainingOrganizationClassRepository, TrainingOrganizationTeacherRepository trainingOrganizationTeacherRepository, DataBaseContext dbContext) : base(dbContext)
		{
			_trainingOrganizationTeacherRepository = trainingOrganizationTeacherRepository;
			_trainingOrganizationClassRepository = trainingOrganizationClassRepository;
			_trainingOrganizationService = trainingOrganizationService;
			_trainingOrganizationClassTeacherMapRepository = trainingOrganizationClassTeacherMapRepository;
			_trainingOrganizationClassLessonRepository = trainingOrganizationClassLessonRepository;
		}

		/// <summary>
		/// 增加一个班级
		/// </summary>
		/// <param name="trainingOrganizationManageUserId">管理人员Id</param>
		/// <param name="request">请求</param>
		public void AddClass(long trainingOrganizationManageUserId, TrainingOrganizationClassAddRequest request)
	    {
		    var trainingOrganization = _trainingOrganizationService.Get(request.TrainingOrganizationId);
		    if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
		    {
				throw new PlatformException("该场馆的管理人员才可以为该场馆添加班级");
		    }
		    if (_trainingOrganizationClassRepository.Any(e =>  e.ClassName == request.Name && e.TrainingOrganizationId == request.TrainingOrganizationId))
			{
				throw new PlatformException("这个班级已经存在了，请不要重复添加");
		    }
		    var classModel = _trainingOrganizationClassRepository.InsertOrUpdate(new TrainingOrganizationClass
		    {
			    ClassName = request.Name,
			    TrainingOrganizationTeacherId = request.TeacherIds.First(),
			    TrainingOrganizationId = trainingOrganization.Id,
				TrainingOrganizationManageUserId = trainingOrganizationManageUserId,
				Dan = request.Dan
			});
		    foreach (var teacherId in request.TeacherIds)
		    {
				_trainingOrganizationClassTeacherMapRepository.Insert(new TrainingOrganizationClassTeacherMap
				{
					TeacherInClassType = TeacherType.Headmaster,
					TrainingOrganizationClassId = classModel.Id,
					TrainingOrganizationTeacherId = teacherId
				});
			}
		    foreach (var time in request.Times)
		    {
			    var list = new List<TrainingOrganizationClassLesson>();
				for (int i = 0; i < 700; i++)
				{
					var day = DateTime.Now.AddDays(i);
					if (day.DayOfWeek == time.DayOfWeek)
					{
						list.Add(new TrainingOrganizationClassLesson
						{
							TrainingOrganizationClassId = classModel.Id,
							StartTime = CtorTime(day, time.StartTime),
							EndTime = CtorTime(day, time.EndTime)
						});
					}
				}
			    _trainingOrganizationClassLessonRepository.BatchInsert(list);
		    }
	    }


        /// <summary>
        /// 查询班级情况
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public QueryResult<TrainingOrganizationClass> Query(TrainingOrganizationClassQuery query)
	    {
		    return _trainingOrganizationClassRepository.Query(query);
	    }

		public void DeleteClass(long trainingOrganizationManageUserId, long classId)
		{
			var cls = _trainingOrganizationClassRepository.Get(classId);
			if (cls == null)
			{
				throw new PlatformException("错误的班级编号");
			}
			if (cls.TrainingOrganizationManageUserId != trainingOrganizationManageUserId)
			{
				throw new PlatformException("管理员才可以删除班级哦");
			}
			_trainingOrganizationClassRepository.Delete(cls);
		}

		/// <summary>
		/// 更新班级信息
		/// </summary>
		/// <param name="trainingOrganizationManageUserId"></param>
		/// <param name="request"></param>
		public void UpdateClass(long trainingOrganizationManageUserId, TrainingOrganizationClassUpdateRequest request)
		{
			var trainingOrganization = _trainingOrganizationService.Get(request.TrainingOrganizationId);
			if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
			{
				throw new PlatformException("该场馆的管理人员才可以为该场馆添加班级");
			}
			if (_trainingOrganizationClassRepository.Any(e => e.Id != request.ClassId && e.ClassName == request.Name && e.TrainingOrganizationId == request.TrainingOrganizationId))
			{
				throw new PlatformException("这个班级已经存在了，请不要重复添加");
			}
			var classModel = _trainingOrganizationClassRepository.InsertOrUpdate(new TrainingOrganizationClass
			{
				Id = request.ClassId,
				ClassName = request.Name,
				TrainingOrganizationTeacherId = request.TeacherIds.First(),
				TrainingOrganizationId = trainingOrganization.Id,
				TrainingOrganizationManageUserId = trainingOrganizationManageUserId,
				Dan = request.Dan
			});
			using (var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>())
			{
				var teachers = dbContext.Set<TrainingOrganizationClassTeacherMap>().Where(e => e.TrainingOrganizationClassId == request.ClassId);
				dbContext.Set<TrainingOrganizationClassTeacherMap>().RemoveRange(teachers);
				dbContext.SaveChanges();
			}
			foreach (var teacherId in request.TeacherIds)
			{
				_trainingOrganizationClassTeacherMapRepository.Insert(new TrainingOrganizationClassTeacherMap
				{
					TeacherInClassType = TeacherType.Headmaster,
					TrainingOrganizationClassId = classModel.Id,
					TrainingOrganizationTeacherId = teacherId
				});
			}
		}
	}
}
