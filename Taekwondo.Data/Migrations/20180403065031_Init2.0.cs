using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class Init20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrainingOrganizationEntId",
                table: "TrainingOrganizations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "GenearchChildMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    Appellation = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChildId = table.Column<long>(nullable: false),
                    GenearchId = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenearchChildMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    IconUrl = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    MenuId = table.Column<long>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassLessonLeaves",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    AuditorId = table.Column<long>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChild = table.Column<long>(nullable: false),
                    IsAgreed = table.Column<bool>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    TrainingOrganizationClassLessonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassLessonLeaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassLessons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    LessonName = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TrainingOrganizationClassId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassLessonSigns",
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
                    table.PrimaryKey("PK_TrainingOrganizationClassLessonSigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassTeacherMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TeacherInClassType = table.Column<int>(nullable: false),
                    TrainingOrganizationClassId = table.Column<long>(nullable: false),
                    TrainingOrganizationTeacherId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassTeacherMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationEnts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationEnts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationTeacherCurriculumVitaes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationTeacherCurriculumVitaes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuId",
                table: "Menus",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenearchChildMaps");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassLessonLeaves");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassLessons");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassLessonSigns");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassTeacherMaps");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationEnts");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationTeacherCurriculumVitaes");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationEntId",
                table: "TrainingOrganizations");
        }
    }
}
