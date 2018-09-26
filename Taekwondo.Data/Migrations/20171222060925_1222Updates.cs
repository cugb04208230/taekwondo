using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class _1222Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TrainingOrganizationTeachers",
                newName: "TrainingOrganizationManageUserId");

            migrationBuilder.RenameColumn(
                name: "TrainingOrganizationSubjectId",
                table: "TrainingOrganizationClasses",
                newName: "TrainingOrganizationManageUserId");

            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "TrainingOrganizationTeachers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrainingOrganizationId",
                table: "TrainingOrganizationClassStudents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationClassStudents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationClassHomeworks",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "TrainingOrganizationTeachers");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationId",
                table: "TrainingOrganizationClassStudents");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationClassStudents");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationClassHomeworks");

            migrationBuilder.RenameColumn(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationTeachers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TrainingOrganizationManageUserId",
                table: "TrainingOrganizationClasses",
                newName: "TrainingOrganizationSubjectId");
        }
    }
}
