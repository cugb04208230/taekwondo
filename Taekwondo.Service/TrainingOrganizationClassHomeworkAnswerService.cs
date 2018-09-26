using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Di;
using Common.Models;
using Common.Push.JiGuang;
using Taekwondo.Data;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class TrainingOrganizationClassHomeworkAnswerService
    {
	    private readonly TrainingOrganizationClassHomeworkAnswerRepository _trainingOrganizationClassHomeworkAnswerRepository;
	    private readonly TrainingOrganizationClassService _trainingOrganizationClassService;

		private readonly GenearchChildService _genearchChildService;

		public TrainingOrganizationClassHomeworkAnswerService(GenearchChildService genearchChildService,
			TrainingOrganizationClassHomeworkAnswerRepository trainingOrganizationClassHomeworkAnswerRepository)
		{
			_trainingOrganizationClassHomeworkAnswerRepository = trainingOrganizationClassHomeworkAnswerRepository;
			_genearchChildService = genearchChildService;
		}

	    public QueryResult<TrainingOrganizationClassHomeworkAnswer> Query(
		    TrainingOrganizationClassHomeworkAnswerQuery query)
	    {
		    return _trainingOrganizationClassHomeworkAnswerRepository.Query(query);
	    }

	    public void Answer(long genearchId, long genearchChildId,long homeworkId, string summary, string files,string images)
	    {
		    var student = _genearchChildService.Get(genearchChildId);
		    var homework = ObjectContainer.Instance.Resolver<TrainingOrganizationClassHomeworkRepository>()
			    .FirstOrDefault(e => e.Id == homeworkId);
		    if (_trainingOrganizationClassHomeworkAnswerRepository.FirstOrDefault(e =>
			        e.GenearchChildId == genearchChildId&&e.TrainingOrganizationClassHomeworkId==homeworkId) != null)
		    {
				throw new PlatformException("该学员已经答题");
		    }
		    _trainingOrganizationClassHomeworkAnswerRepository.Insert(new TrainingOrganizationClassHomeworkAnswer
		    {
			    TrainingOrganizationClassHomeworkId = homeworkId,
			    GenearchId = student.GenearchId,
			    GenearchChildId = student.Id,
			    GenearchChildName = student.GenearchChildName,
			    Summary = summary,
			    Files = files,
				Images = images
		    });
		    var cls = ObjectContainer.Instance.Resolver<TrainingOrganizationClassRepository>()
			    .FirstOrDefault(e => e.Id == homework.TrainingOrganizationClassId);
		    var teacherIds = ObjectContainer.Instance.Resolver<DataBaseContext>().Set<TrainingOrganizationClassTeacherMap>()
			    .Where(e => e.TrainingOrganizationClassId == homework.TrainingOrganizationClassId)
			    .Select(e => e.TrainingOrganizationTeacherId).ToList();
		    var title = $"{cls.ClassName}{student.GenearchChildName}学员提交了“{homework.Title}”作业，快去批改吧。";
			JiGuangExample.Push(title, "", teacherIds);
			ObjectContainer.Instance.Resolver<NoticeService>().Add(teacherIds, title);
		}

	    public void Marking(long teacherId,long answerId,string text,int stars)
	    {
		    var teacher = ObjectContainer.Instance.Resolver<TrainingOrganizationTeacherRepository>().FirstOrDefault(e=>e.TeacherId==teacherId);
		    var answer = _trainingOrganizationClassHomeworkAnswerRepository.Get(answerId);
		    var homework = ObjectContainer.Instance.Resolver<TrainingOrganizationClassHomeworkRepository>()
			    .FirstOrDefault(e => e.Id == answer.TrainingOrganizationClassHomeworkId);
		    answer.Readovered = true;
		    answer.ReadoverText = text;
		    answer.Stars = stars;
		    _trainingOrganizationClassHomeworkAnswerRepository.InsertOrUpdate(answer);
		    var title = $"{teacher.TeacherName}老师批改了你的“{homework.Title}”作业";
			JiGuangExample.Push(title, "",new List<long>{ answer.GenearchId});
		    ObjectContainer.Instance.Resolver<NoticeService>().Add(new List<long> { answer.GenearchId }, title);
		}
    }
}
