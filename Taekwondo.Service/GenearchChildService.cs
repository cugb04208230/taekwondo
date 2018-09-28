using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.Di;
using Common.Models;
using Common.Extensions;
using Common.Util;
using Taekwondo.Data;
using Taekwondo.Data.DTOs.Student;
using Taekwondo.Data.Entities;
using Taekwondo.Data.Enums;

namespace Taekwondo.Service
{
    public class GenearchChildService:BaseService
	{
		private readonly TrainingOrganizationService _trainingOrganizationService;
		private readonly GenearchChildRepository _genearchChildRepository;
		private readonly AccountService _accountService;
		private readonly TrainingOrganizationClassStudentService _trainingOrganizationClassStudentService;
        private readonly GenearchChildMapService _genearchChildMapService;

        public GenearchChildService(TrainingOrganizationClassStudentService trainingOrganizationClassStudentService,AccountService accountService,TrainingOrganizationService trainingOrganizationService, GenearchChildRepository genearchChildRepository, GenearchChildMapService genearchChildMapService, DataBaseContext dbContext) : base(dbContext)
		{
			_trainingOrganizationService = trainingOrganizationService;
			_genearchChildRepository = genearchChildRepository;
			_accountService = accountService;
			_trainingOrganizationClassStudentService = trainingOrganizationClassStudentService;
            _genearchChildMapService = genearchChildMapService;

        }

		/// <summary>
		/// 场馆添加家长名下的学员
		/// </summary>
		/// <param name="trainingOrganizationManageUserId">场馆管理人员Id</param>
		/// <param name="trainingOrganizationId">场馆Id</param>
		/// <param name="mobile">手机号</param>
		/// <param name="name">学员姓名</param>
		/// <param name="appellation">称谓</param>
		/// <param name="idCardNo">学员身份证号</param>
		/// <param name="classId">班级Id</param>
		public void AddGenearchChild(long trainingOrganizationManageUserId, long trainingOrganizationId, string mobile, string name,string appellation, string idCardNo,long classId,long lessionremain,DanType dan)
	    {
		    var trainingOrganization = _trainingOrganizationService.Get(trainingOrganizationId);
		    if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
		    {
			    throw new PlatformException("该场馆的管理人员才可以为该场馆添加学员");
			}

            string[] mobiles = mobile.Split(",");
            string[] appellations = appellation.Split(",");

            for(int i=0;i<mobiles.Length;i++)
            {
                var user = _accountService.AddUserAccount(mobiles[i], "111111", UserType.TrainingOrganizationGenearch, name+ appellations[i]);
 
                var genearchChild = _genearchChildRepository.FirstOrDefault(e => e.GenearchId == user.Id && e.GenearchChildName == name);

                if (genearchChild == null)
                {
                    genearchChild = _genearchChildRepository.Insert(new GenearchChild { GenearchId = user.Id, IdCardNo = idCardNo, GenearchChildName = name, Appellation = appellation,LessionRemain=lessionremain,Dan = dan});
                }
                if (genearchChild != null)
                {
                    _trainingOrganizationClassStudentService.AddClassStudent(trainingOrganizationManageUserId, trainingOrganizationId, classId, genearchChild.Id);

                }
                _genearchChildMapService.AddGenearchMap(genearchChild.Id, user.Id, appellations[i]);
//	            if (lessionremain > 0)
//	            {
//		            using (var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>())
//		            {
//			            var lessonMap = new List<TrainingOrganizationClassStudentLessonMap>();
//			            var lessons = dbContext.TrainingOrganizationClassLessons.Where(e =>
//				            e.TrainingOrganizationClassId == classId && e.StartTime > DateTime.Now).Take(lessionremain.To<int>());
//			            foreach (var trainingOrganizationClassLesson in lessons)
//			            {
//				            lessonMap.Add(new TrainingOrganizationClassStudentLessonMap
//				            {
//					            GenearchChildId = genearchChild.Id,
//								TrainingOrganizationClassLessonId = trainingOrganizationClassLesson.Id
//							});
//			            }
//						dbContext.AddRange(lessonMap);
//			            dbContext.SaveChanges();
//		            }
//	            }
            }
	    }

	    /// <summary>
	    /// 查询学员
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public QueryResult<GenearchChild> Query(GenearchChildQuery query)
	    {
		    var queryResult =  _genearchChildRepository.Query(query);
			//暂时先这样处理
		    using (var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>())
		    {
			    var queryable = from sc in dbContext.Set<TrainingOrganizationClassStudent>()
				    join cls in dbContext.Set<TrainingOrganizationClass>() on sc.TrainingOrganizationClassId equals cls.Id
				    join t in dbContext.Set<TrainingOrganization>() on cls.TrainingOrganizationId equals t.Id
				    select new
				    {
					    stuId = sc.GenearchChildId,
						clsName = cls.ClassName,
						tName = t.TrainingOrganizationName
				    };
			    var list = queryResult.List.ToList();
			    list.ForEach(genearchChild =>
			    {
				    var item = queryable.FirstOrDefault(e => e.stuId == genearchChild.Id);
				    genearchChild.ClassName = item?.clsName;
				    genearchChild.TrainingOrganizationName = item?.tName;
			    });
			    queryResult.List = list;

		    }
		    return queryResult;
	    }

		public GenearchChild Get(long id)
		{
			return _genearchChildRepository.Get(id);
		}
		public StudentQueryOneResponse QueryOne(long id)
		{
			var student = _genearchChildRepository.Get(id);
			if (student == null)
			{
				throw new PlatformException("错误的学员信息");
			}
			var response = ModelMapUtil.AutoMap(student, new StudentQueryOneResponse());
			var parents = from map in DbContext.Set<GenearchChildMap>().Where(e => e.GenearchChildId == id)
				join p in DbContext.Set<UserAccount>() on map.GenearchId equals p.Id
				select new KeyValuePair<string, string>(p.Mobile, map.Appellation);
			response.Parents = parents.ToList();
			return response;
		}


		public void DeleteChild(long trainingOrganizationManageUserId, long classId)
        {
            var cls = _genearchChildRepository.Get(classId);
            if (cls == null)
            {
                throw new PlatformException("错误的班级编号");
            }
            //if (cls.TrainingOrganizationManageUserId != trainingOrganizationManageUserId)
            //{
            //    throw new PlatformException("管理员才可以删除班级哦");
            //}
            _genearchChildRepository.Delete(cls);
        }

        /// <summary>
		/// 场馆更新学员信息
		/// </summary>
		/// <param name="trainingOrganizationManageUserId"></param>
		/// <param name="request"></param>
		public void UpdateChild(long trainingOrganizationManageUserId, StudentUpdateRequest request)
        {
//            var trainingOrganization = _trainingOrganizationService.Get(trainingOrganizationManageUserId);
//            if (trainingOrganization.TrainingOrganizationManagerUserId != trainingOrganizationManageUserId)
//            {
//                throw new PlatformException("该场馆的管理人员才可以为该场馆添加学员");
//            }
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                var genearchChild = _genearchChildRepository.Get(request.Id);
	            if (genearchChild == null)
	            {
		            throw new PlatformException("学员信息错误");
				}
	            if (request.Name.IsNotNullOrEmpty())
                {
                    genearchChild.GenearchChildName = request.Name;
                }
                //if (request.Appellation.IsNotNullOrEmpty())
                //{
                //    genearchChild.Appellation = request.Appellation;
                //}
                //if (request.GenearchId!=0)
                //{
                //    genearchChild.GenearchId = request.GenearchId;
                //}
                if (request.IdCardNo.IsNotNullOrEmpty())
                {
                    genearchChild.IdCardNo = request.IdCardNo;
                }
                
                genearchChild.LessionRemain = request.LessionRemain;
                genearchChild.Dan = request.Dan;
                _genearchChildRepository.Update(genearchChild);
	            var maps = DbContext.Set<GenearchChildMap>().Where(e => e.GenearchChildId == genearchChild.Id);
	            var parentIds = maps.Select(e => e.GenearchId);
				//一个家长多个小孩儿的
	            var maps2 = DbContext.Set<GenearchChildMap>().Where(e =>
		            parentIds.Contains(e.GenearchId) && e.GenearchChildId != genearchChild.Id);
	            maps = maps.Where(e => !maps2.Select(e2 => e2.Id).Contains(e.Id));
				DbContext.Set<GenearchChildMap>().RemoveRange(maps);
	            DbContext.SaveChanges();
				string[] mobiles = request.Mobile.Split(",");
				string[] appellations = request.Appellation.Split(",");

				for (int i = 0; i < mobiles.Length; i++)
				{
					var user = _accountService.AddUserAccount(mobiles[i], "111111", UserType.TrainingOrganizationGenearch, genearchChild.GenearchChildName + appellations[i]);
					_genearchChildMapService.AddGenearchMap(genearchChild.Id, user.Id, appellations[i]);
				}
				transaction.Commit();
            }
        }
    }
}
