﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Taekwondo.Data;

namespace Taekwondo.Data.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20180103091600_HomeAnswerFiles")]
    partial class HomeAnswerFiles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Taekwondo.Data.Entities.Genearch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<string>("Address");

                    b.Property<DateTime>("Birthday");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int>("Gender");

                    b.Property<string>("GenearchName");

                    b.Property<string>("IdCardNo");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Mobile");

                    b.HasKey("Id");

                    b.ToTable("Genearches");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.GenearchChild", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("GenearchChildName");

                    b.Property<long>("GenearchId");

                    b.Property<string>("IdCardNo");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.HasKey("Id");

                    b.ToTable("GenearchChildren");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<string>("Application");

                    b.Property<string>("Callsite");

                    b.Property<string>("Exception");

                    b.Property<string>("Level");

                    b.Property<DateTime>("Logged");

                    b.Property<string>("Logger");

                    b.Property<string>("Message");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.LoginLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime>("ExpiredAt");

                    b.Property<string>("Ip");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Token");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.ToTable("LoginLogs");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.Notice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Message");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Notices");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.SmsLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Ip");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Message");

                    b.Property<string>("Mobile");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("SmsLogs");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("BusinessHoursFromAt");

                    b.Property<DateTime?>("BusinessHoursToAt");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("OpeningDay");

                    b.Property<string>("Pictures");

                    b.Property<string>("Summary");

                    b.Property<long>("TrainingOrganizationManagerUserId");

                    b.Property<string>("TrainingOrganizationName");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizations");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationClass", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<string>("ClassName");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<long>("TrainingOrganizationManageUserId");

                    b.Property<long>("TrainingOrganizationTeacherId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationClasses");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationClassHomework", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Files");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.Property<long>("TrainingOrganizationClassId");

                    b.Property<long>("TrainingOrganizationClassTeacherId");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<long>("TrainingOrganizationManageUserId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationClassHomeworks");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationClassHomeworkAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Files");

                    b.Property<long>("GenearchChildId");

                    b.Property<string>("GenearchChildName");

                    b.Property<long>("GenearchId");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("ReadoverText");

                    b.Property<bool>("Readovered");

                    b.Property<int>("Stars");

                    b.Property<string>("Summary");

                    b.Property<long>("TrainingOrganizationClassHomeworkId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationClassHomeworkAnswers");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationClassHomeworkMarking", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<long>("GenearchChildId");

                    b.Property<long>("GenearchId");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<long>("TeacherId");

                    b.Property<long>("TrainingOrganizationClassHomeworkId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationClassHomeworkMarkings");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationClassStudent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<long>("GenearchChildId");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<long>("TrainingOrganizationClassId");

                    b.Property<int>("TrainingOrganizationClassStudentStatus");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<long>("TrainingOrganizationManageUserId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationClassStudents");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationPrize", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int>("Integral");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Summary");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<string>("TrainingOrganizationPrizeName");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationPrizes");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationPrizeExchangeRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<long>("TrainingOrganizationPrizeId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationPrizeExchangeRecords");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationSubject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<string>("TrainingOrganizationSubjectName");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationSubjects");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.TrainingOrganizationTeacher", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Mobile");

                    b.Property<long>("TeacherId");

                    b.Property<string>("TeacherName");

                    b.Property<long>("TrainingOrganizationId");

                    b.Property<long>("TrainingOrganizationManageUserId");

                    b.HasKey("Id");

                    b.ToTable("TrainingOrganizationTeachers");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.UserAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Mobile");

                    b.Property<string>("Password");

                    b.Property<int>("Status");

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Taekwondo.Data.Entities.VerifyCode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("next value for Ids");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime>("ExpriedAt");

                    b.Property<string>("Ip");

                    b.Property<DateTime?>("LastModifiedAt");

                    b.Property<string>("Mobile");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("VerifyCodes");
                });
#pragma warning restore 612, 618
        }
    }
}
