using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class Init2018041401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ManagerId",
                table: "TrainingOrganizationEnts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StudentLessonMapId",
                table: "TrainingOrganizationClassLessonSigns",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StudentLessonMapId",
                table: "TrainingOrganizationClassLessonLeaves",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "TrainingOrganizationEnts");

            migrationBuilder.DropColumn(
                name: "StudentLessonMapId",
                table: "TrainingOrganizationClassLessonSigns");

            migrationBuilder.DropColumn(
                name: "StudentLessonMapId",
                table: "TrainingOrganizationClassLessonLeaves");
        }
    }
}
