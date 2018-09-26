using System.Linq;
using Common.Data;
using Common.Di;
using Common.Models;
using Common.Push.JiGuang;
using Taekwondo.Data;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class TrainingOrganizationClassHomeworkService:BaseService
    {
	    private readonly TrainingOrganizationClassHomeworkRepository _organizationClassHomeworkRepository;
	    private readonly TrainingOrganizationClassService _trainingOrganizationClassService;
	    private readonly TrainingOrganizationClassHomeworkAnswerService _trainingOrganizationClassHomeworkAnswerService;

		private readonly NoticeService _noticeService;

		public TrainingOrganizationClassHomeworkService(TrainingOrganizationClassHomeworkAnswerService trainingOrganizationClassHomeworkAnswerService,NoticeService noticeService,TrainingOrganizationClassService trainingOrganizationClassService,TrainingOrganizationClassHomeworkRepository organizationClassHomeworkRepository,DataBaseContext dbContext) : base(dbContext)
		{
			_organizationClassHomeworkRepository = organizationClassHomeworkRepository;
			_trainingOrganizationClassService = trainingOrganizationClassService;
			_noticeService = noticeService;
			_trainingOrganizationClassHomeworkAnswerService = trainingOrganizationClassHomeworkAnswerService;
		}

		/// <summary>
		/// 发布家庭作业
		/// </summary>
		/// <param name="teacherId">老师的Id</param>
		/// <param name="classId"></param>
		/// <param name="title"></param>
		/// <param name="summary"></param>
		/// <param name="files"></param>
		/// <param name="images"></param>
		public void PublishHomework(long teacherId,long classId,string title,string summary,string files,string images)
	    {
			var trainingOrganizationClass=_trainingOrganizationClassService.Query(new TrainingOrganizationClassQuery {Id = classId}).List.FirstOrDefault();
		    if (trainingOrganizationClass == null)
		    {
				throw new PlatformException("请选择正确的班级");
			}
		    var teacher = ObjectContainer.Instance.Resolver<UserAccountRepository>().FirstOrDefault(e => e.Id == teacherId);
		    var homework =_organizationClassHomeworkRepository.InsertOrUpdate(new TrainingOrganizationClassHomework
		    {
			    TrainingOrganizationClassId = trainingOrganizationClass.Id,
				TrainingOrganizationId = trainingOrganizationClass.TrainingOrganizationId,
				TrainingOrganizationClassTeacherId = trainingOrganizationClass.TrainingOrganizationTeacherId,
			    TrainingOrganizationManageUserId=trainingOrganizationClass.TrainingOrganizationManageUserId,
				Summary = summary,
				Files = files,
				Title = title,
				Images = images
			});
		    _noticeService.AddClassNotice(classId, $"作业通知:{trainingOrganizationClass.ClassName}发布了新的作业--{homework.Summary},请尽快完成哦。");
		    var studentIds = ObjectContainer.Instance.Resolver<DataBaseContext>().Set<TrainingOrganizationClassStudent>()
			    .Where(e => e.TrainingOrganizationClassId == classId).Select(e => e.GenearchChildId).ToList();

		    JiGuangExample.Push($"{teacher.UserName}老师发不了作业\"{homework.Title}\"", homework.Summary, studentIds);
		    ObjectContainer.Instance.Resolver<NoticeService>().Add(studentIds, $"{teacher.UserName}老师发布了作业\"{homework.Title}\"");
		}

		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
	    public QueryResult<TrainingOrganizationClassHomework> Query(TrainingOrganizationClassHomeworkQuery query)
		{
			return _organizationClassHomeworkRepository.Query(query);
		}

		/// <summary>
		/// 获取作业详情
		/// </summary>
		/// <param name="homeworkId"></param>
		/// <param name="genearchChildId"></param>
		/// <returns></returns>
		public TrainingOrganizationClassHomework Detail(long homeworkId, long?genearchChildId)
		{
			var homework = _organizationClassHomeworkRepository.Get(homeworkId);
			if (homework == null)
			{
				throw new PlatformException("错误的作业编号");
			}
			var answers = _trainingOrganizationClassHomeworkAnswerService.Query(new TrainingOrganizationClassHomeworkAnswerQuery
			{
				TrainingOrganizationClassHomeworkId = homeworkId,
				GenearchChildId = genearchChildId,
				PageSize = int.MaxValue
			});
			homework.Answers = answers.List.ToList();
			return homework;
		}

    }
}
