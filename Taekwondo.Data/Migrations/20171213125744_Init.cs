using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenearchChildren",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    IdCardNo = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenearchChildren", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genearches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    Address = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    IdCardNo = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genearches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    Application = table.Column<string>(nullable: true),
                    Callsite = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Logged = table.Column<DateTime>(nullable: false),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ExpiredAt = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClasses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TrainingOrganizationId = table.Column<long>(nullable: false),
                    TrainingOrganizationSubjectId = table.Column<long>(nullable: false),
                    TrainingOrganizationTeacherId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassHomeworkAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChildId = table.Column<long>(nullable: false),
                    GenearchId = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    ReadoverStatus = table.Column<int>(nullable: false),
                    ReadoverText = table.Column<string>(nullable: true),
                    ReadoverVoice = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    TrainingOrganizationClassHomeworkId = table.Column<long>(nullable: false),
                    Video = table.Column<string>(nullable: true),
                    Voice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassHomeworkAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassHomeworks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TrainingOrganizationClassId = table.Column<long>(nullable: false),
                    TrainingOrganizationClassTeacherId = table.Column<long>(nullable: false),
                    TrainingOrganizationId = table.Column<long>(nullable: false),
                    Video = table.Column<string>(nullable: true),
                    Voice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassHomeworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassStudents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChildId = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TrainingOrganizationClassId = table.Column<long>(nullable: false),
                    TrainingOrganizationClassStudentStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationPrizeExchangeRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TrainingOrganizationPrizeId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationPrizeExchangeRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationPrizes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Integral = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    TrainingOrganizationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationPrizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    BusinessHoursFromAt = table.Column<DateTime>(nullable: true),
                    BusinessHoursToAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    OpeningDay = table.Column<string>(nullable: true),
                    Pictures = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    TrainingOrganizationManagerUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TrainingOrganizationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CurriculumVitae = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    TrainingOrganizationId = table.Column<long>(nullable: false),
                    TrainingOrganizationSubjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationTeachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifyCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyCodes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenearchChildren");

            migrationBuilder.DropTable(
                name: "Genearches");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "LoginLogs");

            migrationBuilder.DropTable(
                name: "SmsLogs");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClasses");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassHomeworkAnswers");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassHomeworks");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassStudents");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationPrizeExchangeRecords");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationPrizes");

            migrationBuilder.DropTable(
                name: "TrainingOrganizations");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationSubjects");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationTeachers");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "VerifyCodes");
        }
    }
}
