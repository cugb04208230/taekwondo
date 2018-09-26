using System.Linq;
using Common;
using Common.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Taekwondo.Data.Entities;

namespace Taekwondo.Data
{
	/// <inheritdoc />
	/// <summary>
	/// 迁移
	/// </summary>
    public class BaseContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
    {
	    DataBaseContext IDesignTimeDbContextFactory<DataBaseContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
            optionsBuilder.UseSqlServer(Settings.GetConfiguration("ConnectionStrings:MsSqlContext"));
            return new DataBaseContext(optionsBuilder.Options);
        }
    }
	/// <inheritdoc />
	/// <summary>
	/// DbContext
	/// </summary>
    public class DataBaseContext:DbContext
    {
		/// <inheritdoc />
		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="options"></param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

		/// <summary>
		/// 版本管理
		/// </summary>
	    public DbSet<AppEdition> AppEditions { get; set; }
		/// <summary>
		/// 家长
		/// </summary>
	    public DbSet<Genearch> Genearches { get; set; }
	    /// <summary>
	    /// 学员
	    /// </summary>
		public DbSet<GenearchChild> GenearchChildren { get; set; }
		/// <summary>
		/// 家长学员关系
		/// </summary>
	    public DbSet<GenearchChildMap> GenearchChildMaps { get; set; }

	    /// <summary>
	    /// 日志
	    /// </summary>
	    public DbSet<Log> Log { set; get; }

	    /// <summary>
	    /// 用户登录记录
	    /// </summary>
	    public DbSet<LoginLog> LoginLogs { get; set; }

	    /// <summary>
	    /// 菜单
	    /// </summary>
	    public DbSet<Menu> Menus { get; set; }
		/// <summary>
		/// 通知
		/// </summary>
		public DbSet<Notice> Notices { get; set; }

	    /// <summary>
	    /// 短信日志
	    /// </summary>

	    public DbSet<SmsLog> SmsLogs { get; set; }

		/// <summary>
		/// 系统设置
		/// </summary>
	    public DbSet<SystemSetting> SystemSettings { set; get; }
		/// <summary>
		/// 场馆
		/// </summary>
		public DbSet<TrainingOrganization> TrainingOrganizations { get; set; }
	    /// <summary>
	    /// 场馆班级
	    /// </summary>
		public DbSet<TrainingOrganizationClass> TrainingOrganizationClasses { get; set; }
	    /// <summary>
	    /// 场馆班级作业
	    /// </summary>
		public DbSet<TrainingOrganizationClassHomework> TrainingOrganizationClassHomeworks { get; set; }
	    /// <summary>
	    /// 场馆班级作业答题
	    /// </summary>
		public DbSet<TrainingOrganizationClassHomeworkAnswer> TrainingOrganizationClassHomeworkAnswers { get; set; }

		/// <summary>
		/// 场馆班级作业答题审阅
		/// </summary>
		public DbSet<TrainingOrganizationClassHomeworkMarking> TrainingOrganizationClassHomeworkMarkings { get; set; }

		/// <summary>
		/// 场馆班级课程
		/// </summary>
	    public DbSet<TrainingOrganizationClassLesson> TrainingOrganizationClassLessons { get; set; }

	    /// <summary>
	    /// 场馆班级学生
	    /// </summary>
	    public DbSet<TrainingOrganizationClassStudent> TrainingOrganizationClassStudents { get; set; }

		/// <summary>
		/// 场馆班级课程请假
		/// </summary>
		public DbSet<TrainingOrganizationClassStudentLessonLeave> TrainingOrganizationClassStudentLessonLeaves { get; set; }

		/// <summary>
		/// 场馆班级课程补课申请
		/// </summary>
		public DbSet<TrainingOrganizationClassStudentLessonMakeUp> TrainingOrganizationClassStudentLessonMakeUps { get; set; }

		/// <summary>
		/// 学生课程列表
		/// </summary>
	    public DbSet<TrainingOrganizationClassStudentLessonMap> TrainingOrganizationClassStudentLessonMaps { get; set; }

	    /// <summary>
	    /// 场馆班级课程签到
	    /// </summary>
	    public DbSet<TrainingOrganizationClassStudentLessonSign> TrainingOrganizationClassStudentLessonSigns { get; set; }


	    /// <summary>
	    /// 场馆老师核班级的映射关系
	    /// </summary>
	    public DbSet<TrainingOrganizationClassTeacherMap> TrainingOrganizationClassTeacherMaps { get; set; }

	    /// <summary>
	    /// 场馆所属企业
	    /// </summary>
	    public DbSet<TrainingOrganizationEnt> TrainingOrganizationEnts { get; set; }
		/// <summary>
		/// 奖品
		/// </summary>
		public DbSet<TrainingOrganizationPrize> TrainingOrganizationPrizes { get; set; }
	    /// <summary>
	    /// 奖品兑换记录
	    /// </summary>
		public DbSet<TrainingOrganizationPrizeExchangeRecord> TrainingOrganizationPrizeExchangeRecords { get; set; }
	    /// <summary>
	    /// 科目
	    /// </summary>
		public DbSet<TrainingOrganizationSubject> TrainingOrganizationSubjects { get; set; }
	    /// <summary>
	    /// 场馆老师
	    /// </summary>
		public DbSet<TrainingOrganizationTeacher> TrainingOrganizationTeachers { get; set; }
		/// <summary>
		/// 问卷
		/// </summary>
		public DbSet<TrainingOrganizationTeacherCurriculumVitae> TrainingOrganizationTeacherCurriculumVitaes { set; get; }
		/// <summary>
		/// 用户账号
		/// </summary>
		public DbSet<UserAccount> UserAccounts { get; set; }
	    /// <summary>
	    /// 验证码
	    /// </summary>
		public DbSet<VerifyCode> VerifyCodes { get; set; }

		public DbSet<校区> 校区 { set; get; }
	    public DbSet<教师> 教师 { set; get; }
	    public DbSet<班级> 班级 { set; get; }
	    public DbSet<家长> 家长 { set; get; }
	    public DbSet<学员> 学员 { set; get; }


		/// <inheritdoc />
		/// <summary>
		/// </summary>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Settings.GetConfiguration("ConnectionStrings:MsSqlContext"));
        }

	    /// <inheritdoc />
	    /// <summary>
	    /// </summary>
		protected override void OnModelCreating(ModelBuilder builder)
        {
			foreach (var type in builder.Model.GetEntityTypes())
			{
				var properties = type.GetType().GetProperties().Where(e => !e.HasNotMapped()).ToList();
				foreach (var mutableProperty in properties)
				{
					builder.Entity(type.Name).Ignore(mutableProperty.Name);
				}
                builder.Entity(type.Name, b =>
                {
                    b.Property<long>("Id").HasDefaultValueSql("next value for Ids");
                });
            }
            base.OnModelCreating(builder);
        }

		/// <summary>
		/// 获取序列
		/// </summary>
		/// <returns></returns>
		public long GetIds()
        {
            
            var concurrencyDetector = Database.GetService<IConcurrencyDetector>();
            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build("SELECT NEXT VALUE FOR Ids As Id");
                RelationalDataReader reader = rawSqlCommand.ExecuteReader(Database.GetService<IRelationalConnection>());
                while (reader.DbDataReader.Read())
                {
                    object[] values = new object[1];
                    reader.DbDataReader.GetValues(values);
                    return long.Parse(values[0].ToString());
                }
                return 0;
            }
        }
    }
}
