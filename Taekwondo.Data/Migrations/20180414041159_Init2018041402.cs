using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class Init2018041402 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingOrganizationClassLessonSigns",
                table: "TrainingOrganizationClassLessonSigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingOrganizationClassLessonLeaves",
                table: "TrainingOrganizationClassLessonLeaves");

            migrationBuilder.RenameTable(
                name: "TrainingOrganizationClassLessonSigns",
                newName: "TrainingOrganizationClassStudentLessonSigns");

            migrationBuilder.RenameTable(
                name: "TrainingOrganizationClassLessonLeaves",
                newName: "TrainingOrganizationClassStudentLessonLeaves");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingOrganizationClassStudentLessonSigns",
                table: "TrainingOrganizationClassStudentLessonSigns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingOrganizationClassStudentLessonLeaves",
                table: "TrainingOrganizationClassStudentLessonLeaves",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassStudentLessonMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChild = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TrainingOrganizationClassLessonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassStudentLessonMaps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassStudentLessonMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingOrganizationClassStudentLessonSigns",
                table: "TrainingOrganizationClassStudentLessonSigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingOrganizationClassStudentLessonLeaves",
                table: "TrainingOrganizationClassStudentLessonLeaves");

            migrationBuilder.RenameTable(
                name: "TrainingOrganizationClassStudentLessonSigns",
                newName: "TrainingOrganizationClassLessonSigns");

            migrationBuilder.RenameTable(
                name: "TrainingOrganizationClassStudentLessonLeaves",
                newName: "TrainingOrganizationClassLessonLeaves");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingOrganizationClassLessonSigns",
                table: "TrainingOrganizationClassLessonSigns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingOrganizationClassLessonLeaves",
                table: "TrainingOrganizationClassLessonLeaves",
                column: "Id");
        }
    }
}
