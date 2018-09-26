using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class InitModel2018042203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassStudentLessonMakeUps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChildId = table.Column<long>(nullable: false),
                    GenearchId = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    OriginalStudentLessonMapId = table.Column<long>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StudentLessonMapId = table.Column<long>(nullable: false),
                    TrainingOrganizationClassLessonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassStudentLessonMakeUps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassStudentLessonMakeUps");
        }
    }
}
