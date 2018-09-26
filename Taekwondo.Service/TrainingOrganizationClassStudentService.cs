using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Di;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Taekwondo.Data;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class TrainingOrganizationClassStudentService
	{
		private readonly TrainingOrganizationClassStudentRepository _trainingOrganizationClassStudentRepository;
		private readonly TrainingOrganizationClassRepository _trainingOrganizationClassRepository;
		public TrainingOrganizationClassStudentService(TrainingOrganizationClassRepository trainingOrganizationClassRepository,TrainingOrganizationClassStudentRepository trainingOrganizationClassStudentRepository)
		{
			_trainingOrganizationClassStudentRepository = trainingOrganizationClassStudentRepository;
			_trainingOrganizationClassRepository = trainingOrganizationClassRepository;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="trainingOrganizationManageUserId"></param>
		/// <param name="trainingOrganizationId"></param>
		/// <param name="classId"></param>
		/// <param name="genearchChildId"></param>
		public void AddClassStudent(long trainingOrganizationManageUserId, long trainingOrganizationId, long classId,
			long genearchChildId)
		{
			var trainingOrganizationClass = _trainingOrganizationClassRepository.Get(classId);
			if (trainingOrganizationClass == null)
			{
				throw new PlatformException("该班级不存在");
			}
			if (trainingOrganizationClass.TrainingOrganizationId != trainingOrganizationId ||
			    trainingOrganizationClass.TrainingOrganizationManageUserId != trainingOrganizationManageUserId)
			{
				throw new PlatformException("班级场馆信息不存在");
			}
			var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>();
			var genearchChild = dbContext.GenearchChildren.FirstOrDefault(e => e.Id == genearchChildId);
			var lessons = dbContext.TrainingOrganizationClassLessons.Where(e => e.StartTime > DateTime.Now && e.TrainingOrganizationClassId == classId).OrderBy(e => e.StartTime)
			.Take((int) (genearchChild?.LessionRemain ?? 0));
			if (lessons.Any())
			{
				var maps = new List<TrainingOrganizationClassStudentLessonMap>();
				foreach (var lesson in lessons)
				{
					var map = new TrainingOrganizationClassStudentLessonMap
					{
						GenearchChildId = genearchChildId,
						TrainingOrganizationClassLessonId = lesson.Id
					};
					maps.Add(map);
				}
				dbContext.AddRange(maps);
				dbContext.SaveChanges();
			}
			_trainingOrganizationClassStudentRepository.Insert(new TrainingOrganizationClassStudent
			{
				TrainingOrganizationManageUserId = trainingOrganizationManageUserId,
				TrainingOrganizationId = trainingOrganizationId,
				TrainingOrganizationClassId = classId,
				GenearchChildId = genearchChildId,
				TrainingOrganizationClassStudentStatus = (int)TrainingOrganizationClassStudentStatus.Start
			});
		}

		public QueryResult<TrainingOrganizationClassStudent> Query(TrainingOrganizationClassStudentQuery query)
		{
			return _trainingOrganizationClassStudentRepository.Query(query);
		}
	}
}
