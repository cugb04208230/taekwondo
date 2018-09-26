using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Taekwondo.Data;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class NoticeService:BaseService
    {
	    private readonly NoticeRepository _noticeRepository;
	    private readonly TrainingOrganizationClassStudentService _trainingOrganizationClassStudentService;
	    public NoticeService(TrainingOrganizationClassStudentService trainingOrganizationClassStudentService,NoticeRepository noticeRepository,DataBaseContext dbContext) : base(dbContext)
	    {
		    _noticeRepository = noticeRepository;
		    _trainingOrganizationClassStudentService = trainingOrganizationClassStudentService;
	    }

	    public void Add(List<long> userIds, string message)
	    {
		    foreach (var userId in userIds)
		    {
			    _noticeRepository.Insert(new Notice {Message = message,UserId = userId});
		    }
	    }

		public void Add(Notice notice)
	    {
		    _noticeRepository.Insert(notice);
	    }

	    public void AddClassNotice(long classId, string message)
	    {
		    var students = _trainingOrganizationClassStudentService.Query(new TrainingOrganizationClassStudentQuery{TrainingOrganizationClassId = classId,PageIndex = int.MaxValue});
		    var notices = students.List.Select(e => new Notice
		    {
			    IsRead = false,
			    Message = message,
			    UserId = e.GenearchChildId
		    }).ToList();
		    _noticeRepository.BatchInsert(notices);
	    }

	    public QueryResult<Notice> Query(NoticeQuery query)
	    {
		    var rst = _noticeRepository.Query(query);
		    if (rst.List.Any())
		    {
			    rst.List.Select(e => e.Id).ToList().ForEach(ReadNotice);
		    }
		    return rst;
	    }

	    public void ReadNotice(long id)
	    {
		    var notice = _noticeRepository.Get(id);
		    if (notice != null)
		    {
			    notice.IsRead = true;
			    _noticeRepository.Update(notice);
		    }
	    }
    }
}
