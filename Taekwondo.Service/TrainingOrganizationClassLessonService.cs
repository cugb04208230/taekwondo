using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Di;
using Common.Models;
using Common.Push.JiGuang;
using Common.Util;
using Newtonsoft.Json;
using Taekwondo.Data;
using Taekwondo.Data.DTOs;
using Taekwondo.Data.DTOs.Lesson;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class TrainingOrganizationClassLessonService:BaseService
    {
	    private readonly TrainingOrganizationClassLessonRepository _lessonRepository;
	    private readonly TrainingOrganizationClassStudentLessonMapRepository _studentLessonMapRepository;
	    private readonly TrainingOrganizationClassLessonLeaveRepository _lessonLeaveRepository;
	    private readonly TrainingOrganizationClassLessonSignRepository _lessonSignRepository;
		private readonly GenearchChildRepository _genearchChildRepository;
	    private readonly TrainingOrganizationClassStudentLessonMakeUpRepository _lessonMakeUpRepository;
	    private readonly TrainingOrganizationClassStudentRepository _classStudentRepository;

		public TrainingOrganizationClassLessonService(TrainingOrganizationClassStudentRepository classStudentRepository,DataBaseContext dbContext, TrainingOrganizationClassLessonRepository lessonRepository, TrainingOrganizationClassStudentLessonMapRepository studentLessonMapRepository, TrainingOrganizationClassLessonLeaveRepository lessonLeaveRepository, GenearchChildRepository genearchChildRepository, TrainingOrganizationClassStudentLessonMakeUpRepository lessonMakeUpRepository, TrainingOrganizationClassLessonSignRepository lessonSignRepository) : base(dbContext)
		{
			_lessonRepository = lessonRepository;
			_studentLessonMapRepository = studentLessonMapRepository;
			_lessonLeaveRepository = lessonLeaveRepository;
			_genearchChildRepository = genearchChildRepository;
			_lessonMakeUpRepository = lessonMakeUpRepository;
			_lessonSignRepository = lessonSignRepository;
			_classStudentRepository = classStudentRepository;
		}

		/// <summary>
		/// 新增班级课程
		/// </summary>
		/// <param name="request"></param>
	    public void LessonAdd(LessonAddRequest request)
	    {
		    _lessonRepository.Insert(new TrainingOrganizationClassLesson
		    {
			    EndTime = request.EndTime,
			    LessonName = "",
			    StartTime = request.StartTime,
			    TrainingOrganizationClassId = request.ClassId
		    });
	    }

		/// <summary>
		/// 课程预约
		/// </summary>
		/// <param name="request"></param>
	    public void LessonReserve(LessonReserveRequest request)
	    {
		    foreach (var requestLessonId in request.LessonIds)
		    {
				_studentLessonMapRepository.Insert(new TrainingOrganizationClassStudentLessonMap
			    {
				    GenearchChildId = request.StudentId,
				    TrainingOrganizationClassLessonId = requestLessonId
			    });
		    }
	    }

		/// <summary>
		/// 学生课程列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
	    public StudentLessonListResponse StudentLessonList(StudentLessonListRequest request)
	    {
		    request.ResponseModel=new StudentLessonListResponse();
		    var student = DbContext.Set<GenearchChild>().FirstOrDefault(e => e.Id == request.StudentId);
		    if (student == null)
		    {
				throw new PlatformException("学员信息错误");
		    }
		    request.ResponseModel.LessionRemain = student.LessionRemain??0;
		    var query = from map in DbContext.Set<TrainingOrganizationClassStudentLessonMap>()
				    .Where(e => e.GenearchChildId == request.StudentId)
			    join lesson in DbContext.Set<TrainingOrganizationClassLesson>() on map
					    .TrainingOrganizationClassLessonId equals
				    lesson.Id
			    join sign in DbContext.Set<TrainingOrganizationClassStudentLessonSign>() on map.Id equals sign.StudentLessonMapId
				    into signs
			    from sign in signs.DefaultIfEmpty()
			    join leave in DbContext.Set<TrainingOrganizationClassStudentLessonLeave>() on map.Id equals leave
				    .StudentLessonMapId into leaves
			    from leave in leaves.DefaultIfEmpty()
			    join makeUp in DbContext.Set<TrainingOrganizationClassStudentLessonMakeUp>() on map.Id equals makeUp
				    .OriginalStudentLessonMapId into makeUps
			    from makeUp in makeUps.DefaultIfEmpty()
			    join hasMakeUp in DbContext.Set<TrainingOrganizationClassStudentLessonMakeUp>() on map.Id equals hasMakeUp
				    .OriginalStudentLessonMapId into hasMakeUps
			    from hasMakeUp in hasMakeUps.DefaultIfEmpty()
			    select new StudentLessonListItemDto
			    {
				    Id = map.Id,
				    StartTime = lesson.StartTime,
				    EndTime = lesson.EndTime,
				    IsLeave = leave != null,
				    IsMakeUp = makeUp != null,
				    IsSign = sign != null,
				    HasMakeUp = hasMakeUp != null,
					LeaveStatus = leave!=null?leave.Status:StudentLessonLeaveStatus.NotSubmit,
					MakeUpStatus = makeUp!=null?makeUp.Status:StudentLessonMakeUpStatus.NotSubmit
				};
		    var list1 = query.Where(e => e.StartTime > DateTime.Now.Date).OrderBy(e=>e.StartTime).Take(10).ToList();
			request.ResponseModel.Total = query.Count(e => e.StartTime > DateTime.Now.Date);
		    request.ResponseModel.List = list1.OrderBy(e=>e.Id).ToList();
		    if (request.PageIndex == 1)
		    {
			    var list2 = query.Where(e => e.StartTime < DateTime.Now.Date).OrderByDescending(e => e.StartTime).Take(10).ToList();
			    request.ResponseModel.BeforeList = list2.OrderBy(e => e.Id).ToList();
			}
		    return request.ResponseModel;
	    }

	    public void StudentLessonSign(StudentLessonSignRequest request)
	    {
           var stu=   _classStudentRepository.FirstOrDefault(o => o.Id == request.StudentId);

            if (stu != null) {
                var child = _genearchChildRepository.FirstOrDefault(o => o.GenearchId == stu.GenearchChildId);
                if (child != null && child.LessionRemain != null && child.LessionRemain > 0)
                {
                    child.LessionRemain -= 1;
                    _genearchChildRepository.Update(child);
                }
            }
            var map = _studentLessonMapRepository.Get(request.Id);
		    if (map == null)
		    {
				return;
		    }
		    if (!_lessonSignRepository.Any(e => e.GenearchChild == request.StudentId && request.Id == e.StudentLessonMapId))
			{
				_lessonSignRepository.Insert(new TrainingOrganizationClassStudentLessonSign
				{
					GenearchChild = request.StudentId,
					StudentLessonMapId = request.Id,
					TrainingOrganizationClassLessonId = map.TrainingOrganizationClassLessonId
				});
			}
	    }

	    /// <summary>
		/// 请假申请
		/// </summary>
		/// <param name="request"></param>
	    public void StudentLessonLeave(StudentLessonLeaveRequest request)
	    {
		    var map = _studentLessonMapRepository.FirstOrDefault(e =>
			    e.GenearchChildId == request.StudentId && e.Id == request.Id);
		    if (map == null)
			{
				throw new PlatformException("还没有预约该课程");
			}
		    var lesson = _lessonRepository.FirstOrDefault(e => e.Id == map.TrainingOrganizationClassLessonId);
		    if (lesson == null)
		    {
			    throw new PlatformException("错误的课程编号！");
		    }
		    if (lesson.StartTime <= DateTime.Now)
		    {
			    throw new PlatformException("课程开始后不能请假哦！");
		    }
			_lessonLeaveRepository.Insert(new TrainingOrganizationClassStudentLessonLeave
		    {
			    TrainingOrganizationClassLessonId = lesson.Id,
			    StudentLessonMapId = map.Id,
			    GenearchChildId = request.StudentId,
			    Reason = request.Remark,
				Status = StudentLessonLeaveStatus.Submit
		    });
		    var cls = ObjectContainer.Instance.Resolver<TrainingOrganizationClassRepository>()
			    .FirstOrDefault(e => e.Id == lesson.TrainingOrganizationClassId);
		    var student = ObjectContainer.Instance.Resolver<GenearchChildRepository>()
			    .FirstOrDefault(e => e.Id == request.StudentId);
		    var teacherIds = ObjectContainer.Instance.Resolver<DataBaseContext>().Set<TrainingOrganizationClassTeacherMap>()
			    .Where(e => e.TrainingOrganizationClassId == lesson.TrainingOrganizationClassId)
			    .Select(e => e.TrainingOrganizationTeacherId).ToList();
			var title =
			    $"{cls.ClassName}班{student.GenearchChildName}学生申请请假，请假时间“{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(lesson.StartTime.DayOfWeek)} {lesson.StartTime:HH:mm}-{lesson.EndTime:HH:mm}";
			JiGuangExample.Push(title,"", teacherIds);
			ObjectContainer.Instance.Resolver<NoticeService>().Add(teacherIds,title);
	    }

		/// <summary>
		/// 获取可预约补课的班级
		/// </summary>
	    public StudentLessonReserveListResponse StudentLessonReserveList(StudentLessonReserveListRequest request)
		{
			request.ResponseModel=new StudentLessonReserveListResponse();
			var student = _genearchChildRepository.Get(request.StudentId);
			if (student == null)
			{
				throw new PlatformException("错误的学员编号");
			}
			var classIds = _classStudentRepository.Query(e => e.GenearchChildId == student.Id).Select(e=>e.TrainingOrganizationClassId);
			var queryable = from lesson in DbContext.Set<TrainingOrganizationClassLesson>()
				join cl in DbContext.Set<TrainingOrganizationClass>().Where(e=> classIds.Contains(e.Id)).Where(e => e.Dan == student.Dan || e.Dan == DanType.混)
					on lesson.TrainingOrganizationClassId equals cl.Id
				join map in DbContext.Set<TrainingOrganizationClassStudentLessonMap>().Where(e=>e.GenearchChildId==request.StudentId) on lesson.Id equals
					map.TrainingOrganizationClassLessonId into maps
				from map in maps.DefaultIfEmpty()
				select new StudentLessonReserveListItemDto
				{
					Id = lesson.Id,
					Dan = cl.Dan,
					ClassName = cl.ClassName,
					StartTime = lesson.StartTime,
					EndTime = lesson.EndTime,
					HasReserved = map!=null
				};
			queryable = queryable.Where(e => !e.HasReserved);
			if (request.DayOfWeek.HasValue)
			{
				queryable = queryable.Where(e => e.StartTime.DayOfWeek == request.DayOfWeek);
			}
			if (request.TimeOfDayType.HasValue)
			{
				switch (request.TimeOfDayType)
				{
					case TimeOfDayType.Morning:
						queryable = queryable.Where(e => e.StartTime.Hour < 12);
						break;
					case TimeOfDayType.Afternoon:
						queryable = queryable.Where(e => e.StartTime.Hour < 18&& e.StartTime.Hour >= 12);
						break;
					case TimeOfDayType.Evening:
						queryable = queryable.Where(e => e.StartTime.Hour > 18);
						break;
				}
			}
			request.ResponseModel.Total = queryable.Count();
			request.ResponseModel.List = queryable.Skip(request.Skip).Take(request.Take).ToList();
			return request.ResponseModel;
		}

	    public void StudentLessonMakeUp(StudentLessonMakeUpRequest request)
	    {
		    var map = _studentLessonMapRepository.FirstOrDefault(e => e.Id == request.OriginalLessonId);
		    if (map == null)
		    {
				throw new PlatformException("原始课程不存在");
		    }
		    if (!_lessonMakeUpRepository.Any(e =>
			    e.TrainingOrganizationClassLessonId == request.LessonId && e.GenearchChildId == request.StudentId &&
			    e.Status == StudentLessonMakeUpStatus.Applied))
			{
				_lessonMakeUpRepository.Insert(new TrainingOrganizationClassStudentLessonMakeUp
				{
					TrainingOrganizationClassLessonId = request.LessonId,
					GenearchChildId = request.StudentId,
					Status = StudentLessonMakeUpStatus.Applied,
					OriginalStudentLessonMapId = request.OriginalLessonId
				});

			}
		    var lessonOrg = _lessonRepository.FirstOrDefault(e => e.Id == map.TrainingOrganizationClassLessonId);
			var lessonNew = _lessonRepository.FirstOrDefault(e => e.Id == map.TrainingOrganizationClassLessonId);
			var clsOrg = ObjectContainer.Instance.Resolver<TrainingOrganizationClassRepository>()
			    .FirstOrDefault(e => e.Id == lessonOrg.TrainingOrganizationClassId);
		    var clsNew = ObjectContainer.Instance.Resolver<TrainingOrganizationClassRepository>()
			    .FirstOrDefault(e => e.Id == lessonNew.TrainingOrganizationClassId);
			var student = ObjectContainer.Instance.Resolver<GenearchChildRepository>()
			    .FirstOrDefault(e => e.Id == request.StudentId);
		    var teacherIds = ObjectContainer.Instance.Resolver<DataBaseContext>().Set<TrainingOrganizationClassTeacherMap>()
			    .Where(e => e.TrainingOrganizationClassId == lessonNew.TrainingOrganizationClassId|| e.TrainingOrganizationClassId == lessonOrg.TrainingOrganizationClassId)
			    .Select(e => e.TrainingOrganizationTeacherId).ToList();
			var title =
			    $"{clsOrg.ClassName}班{student.GenearchChildName}学生申请{clsNew.ClassName}班补课，补课时间“{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(lessonNew.StartTime.DayOfWeek)} {lessonNew.StartTime:HH:mm}-{lessonNew.EndTime:HH:mm}";
		    JiGuangExample.Push(title, "", teacherIds);
		    ObjectContainer.Instance.Resolver<NoticeService>().Add(teacherIds, title);

		}

	    /// <summary>
		/// 老师获取班级课程
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public TeacherClassLessonListResponse TeacherLessonList(TeacherClassLessonListRequest request)
	    {
			request.ResponseModel=new TeacherClassLessonListResponse();
		    var queryable = DbContext.Set<TrainingOrganizationClassLesson>().Where(e =>
				    e.TrainingOrganizationClassId == request.ClassId && e.StartTime >= DateTime.Now.Date)
			    .Select(e => new TeacherClassLessonListItemDto
			    {
				    Id = e.Id,
				    StartTime = e.StartTime,
				    EndTime = e.EndTime
			    });
		    request.ResponseModel.Total = queryable.Count();
		    request.ResponseModel.List =queryable.OrderBy(e => e.StartTime).Skip(request.Skip).Take(request.Take).ToList();
		    return request.ResponseModel;
	    }

		/// <summary>
		/// 老师获取班级签到列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
	    public TeacherClassLessonSignListResponse TeacherClassLessonSignList(TeacheClassLessonSignListRequest request)
	    {
		    var queryable = from map in DbContext.Set<TrainingOrganizationClassStudentLessonMap>()
				    .Where(e => e.TrainingOrganizationClassLessonId == request.LessonId)
			    join student in DbContext.Set<GenearchChild>() on map.GenearchChildId equals student.Id
			    join sign in DbContext.Set<TrainingOrganizationClassStudentLessonSign>() on map.Id equals sign.StudentLessonMapId
				    into signs
			    from sign in signs.DefaultIfEmpty()
			    join leave in DbContext.Set<TrainingOrganizationClassStudentLessonLeave>() on map.Id equals leave
				    .StudentLessonMapId into leaves
			    from leave in leaves.DefaultIfEmpty()
			    join makeUp in DbContext.Set<TrainingOrganizationClassStudentLessonMakeUp>() on map.Id equals makeUp
				    .StudentLessonMapId into makeUps
			    from makeUp in makeUps.DefaultIfEmpty()
			    select new TeacherClassLessonSignListItemDto
			    {
				    StudentId = student.Id,
				    StudentName = student.GenearchChildName,
				    HasLeaved = leave != null,
				    HasSigned = sign != null,
				    IsMakeUp = makeUp != null
			    };
			request.ResponseModel=new TeacherClassLessonSignListResponse
			{
				Total = queryable.Count(),
				List = queryable.ToList()
			};
		    return request.ResponseModel;
	    }

	    /// <summary>
	    /// 老师获取审批列表
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    public TeacherClassApprovalListResponse TeacherClassApprovalList(TeacherClassApprovalListRequest request)
	    {
		    var queryable =
		    (from leave in DbContext.Set<TrainingOrganizationClassStudentLessonLeave>()
			    join map in DbContext.Set<TrainingOrganizationClassStudentLessonMap>() on leave.StudentLessonMapId equals map.Id
			    join lesson in DbContext.Set<TrainingOrganizationClassLesson>()
					    .Where(e => e.TrainingOrganizationClassId == request.ClassId) on leave.TrainingOrganizationClassLessonId equals
				    lesson.Id
			    join cl in DbContext.Set<TrainingOrganizationClass>() on lesson.TrainingOrganizationClassId equals cl.Id
			    join student in DbContext.Set<GenearchChild>() on leave.GenearchChildId equals student.Id
			    select new TeacherClassApprovalListItemDto
			    {
				    Id = leave.Id,
				    ClassName = cl.ClassName,
				    Remark = leave.Reason,
				    StudentId = student.Id,
				    StudentName = student.GenearchChildName,
				    TargetStartTime = lesson.StartTime,
				    TargetEndTime = lesson.EndTime,
				    Type = ApprovalType.Leave,
				    Dan = student.Dan
			    }).Union(
			    from makeUp in DbContext.Set<TrainingOrganizationClassStudentLessonMakeUp>()
			    join lesson in DbContext.Set<TrainingOrganizationClassLesson>()
					    .Where(e => e.TrainingOrganizationClassId == request.ClassId) on makeUp.TrainingOrganizationClassLessonId
				    equals lesson.Id
			    join cl in DbContext.Set<TrainingOrganizationClass>() on lesson.TrainingOrganizationClassId equals cl.Id
			    join student in DbContext.Set<GenearchChild>() on makeUp.GenearchChildId equals student.Id
			    select new TeacherClassApprovalListItemDto
			    {
				    Id = makeUp.Id,
				    ClassName = cl.ClassName,
				    StudentId = student.Id,
				    StudentName = student.GenearchChildName,
				    TargetStartTime = lesson.StartTime,
				    TargetEndTime = lesson.EndTime,
				    Type = ApprovalType.MakeUp,
				    Dan = student.Dan,
				    Status =makeUp.Status

				});
		    request.ResponseModel = new TeacherClassApprovalListResponse
		    {
			    List = queryable.OrderByDescending(e=>e.Id).Skip(request.Skip).Take(request.Take).ToList(),
			    Total = queryable.Count()
		    };
		    return request.ResponseModel;
	    }

	    /// <summary>
		/// 老师审批
		/// </summary>
		/// <param name="request"></param>
	    public void TeacherClassApproval(TeacherClassApprovalRequest request)
	    {
		    switch (request.Type)
		    {
				case ApprovalType.Leave:
					var leave = _lessonLeaveRepository.Get(request.Id);
					if (leave == null)
					{
						throw new PlatformException("错误的编号");
					}
					leave.IsAgreed = true;
					leave.Status = request.IsAgree?StudentLessonLeaveStatus.Agreed:StudentLessonLeaveStatus.Refused;
					_lessonLeaveRepository.Update(leave);
					break;
				case ApprovalType.MakeUp:
					var makeUp = _lessonMakeUpRepository.Get(request.Id);
					if (makeUp == null)
					{
						throw new PlatformException("错误的编号");
					}
					var lesson = ObjectContainer.Instance.Resolver<TrainingOrganizationClassLessonRepository>()
						.FirstOrDefault(e => e.Id == makeUp.TrainingOrganizationClassLessonId);
					var cls = ObjectContainer.Instance.Resolver<TrainingOrganizationClassRepository>()
						.FirstOrDefault(e => e.Id == lesson.TrainingOrganizationClassId);
					makeUp.Status = request.IsAgree
						? StudentLessonMakeUpStatus.ApprovalSuccess
						: StudentLessonMakeUpStatus.ApprovalFail;
					if (request.IsAgree)
					{
						var map = _studentLessonMapRepository.Insert(new TrainingOrganizationClassStudentLessonMap
						{
							TrainingOrganizationClassLessonId = makeUp.TrainingOrganizationClassLessonId,
							GenearchChildId = makeUp.GenearchChildId
						});
						makeUp.StudentLessonMapId = map.Id;
					}
					_lessonMakeUpRepository.Update(makeUp);
					var title =
						$"{cls.ClassName}补课申请已 {(request.IsAgree ? ("同意，补课时间" + $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(lesson.StartTime.DayOfWeek)} {lesson.StartTime:HH:mm}-{lesson.EndTime:HH:mm}") : "已拒绝")}";
					JiGuangExample.Push(title, "", new List<long>{ makeUp.GenearchId});
					ObjectContainer.Instance.Resolver<NoticeService>().Add(new List<long> { makeUp.GenearchId }, title);
					break;
		    }
	    }


	    public void TeacherLessonSign(TeacherLessonSignRequest request)
	    {
		    var maps = _studentLessonMapRepository.Query(e=>e.TrainingOrganizationClassLessonId==request.Id);
		    var studentIds = request.StudentIds.Split(",").Select(long.Parse).ToList();
		    foreach (var studentId in studentIds)
		    {
			    var map = maps.FirstOrDefault(e => e.GenearchChildId == studentId);
			    if (map == null)
			    {
					continue;
			    }
			    if (!_lessonSignRepository.Any(e => e.GenearchChild == studentId && request.Id == e.StudentLessonMapId))
				{
					_lessonSignRepository.Insert(new TrainingOrganizationClassStudentLessonSign
					{
						GenearchChild = studentId,
						StudentLessonMapId = map.Id,
						TrainingOrganizationClassLessonId = map.TrainingOrganizationClassLessonId
					});
				}
			}
	    }

	    public ClassLessonListResponse ClassLessonList(ClassLessonListRequest request)
	    {

		    var responseModel = new ClassLessonListResponse();
		    var queryable = DbContext.Set<TrainingOrganizationClassLesson>().Where(e =>
				    e.TrainingOrganizationClassId == request.ClassId && e.StartTime >= DateTime.Now.Date)
			    .Select(e => new ClassLessonListItemDto
			    {
				    Id = e.Id,
				    StartTime = e.StartTime,
				    EndTime = e.EndTime
			    });
		    responseModel.Total = queryable.Count();
		    responseModel.List = queryable.OrderBy(e => e.StartTime).Skip(request.Skip).Take(request.Take).ToList();
		    return request.ResponseModel;
		}

	    public void ClassDelete(ClassLessonDeleteRequest request)
	    {
			_lessonRepository.Delete(request.Id);
	    }

	    public void ClassUpdate(ClassLessonUpdateRequest request)
	    {
		    var lesson = _lessonRepository.Get(request.Id);
		    if (lesson == null)
		    {
				throw new PlatformException("错误的课程编号");
		    }
		    lesson.StartTime = CtorTime(request.Day, request.StartTime);
			lesson.EndTime = CtorTime(request.Day, request.EndTime);
		    _lessonRepository.Update(lesson);
	    }

    }


}
