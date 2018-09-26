using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "TrainingOrganizationClassHomeworks");

            migrationBuilder.DropColumn(
                name: "Voice",
                table: "TrainingOrganizationClassHomeworks");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "TrainingOrganizationClassHomeworkAnswers");

            migrationBuilder.DropColumn(
                name: "ReadoverVoice",
                table: "TrainingOrganizationClassHomeworkAnswers");

            migrationBuilder.RenameColumn(
                name: "Voice",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "GenearchChildName");

            migrationBuilder.RenameColumn(
                name: "Video",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "Files");

            migrationBuilder.RenameColumn(
                name: "ReadoverStatus",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "Stars");

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "TrainingOrganizationTeachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingOrganizationSubjectName",
                table: "TrainingOrganizationSubjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingOrganizationName",
                table: "TrainingOrganizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingOrganizationPrizeName",
                table: "TrainingOrganizationPrizes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Readovered",
                table: "TrainingOrganizationClassHomeworkAnswers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "TrainingOrganizationClasses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenearchName",
                table: "Genearches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenearchChildName",
                table: "GenearchChildren",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "TrainingOrganizationTeachers");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationSubjectName",
                table: "TrainingOrganizationSubjects");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationName",
                table: "TrainingOrganizations");

            migrationBuilder.DropColumn(
                name: "TrainingOrganizationPrizeName",
                table: "TrainingOrganizationPrizes");

            migrationBuilder.DropColumn(
                name: "Readovered",
                table: "TrainingOrganizationClassHomeworkAnswers");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "TrainingOrganizationClasses");

            migrationBuilder.DropColumn(
                name: "GenearchName",
                table: "Genearches");

            migrationBuilder.DropColumn(
                name: "GenearchChildName",
                table: "GenearchChildren");

            migrationBuilder.RenameColumn(
                name: "Stars",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "ReadoverStatus");

            migrationBuilder.RenameColumn(
                name: "GenearchChildName",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "Voice");

            migrationBuilder.RenameColumn(
                name: "Files",
                table: "TrainingOrganizationClassHomeworkAnswers",
                newName: "Video");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "TrainingOrganizationClassHomeworks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voice",
                table: "TrainingOrganizationClassHomeworks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "TrainingOrganizationClassHomeworkAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReadoverVoice",
                table: "TrainingOrganizationClassHomeworkAnswers",
                nullable: true);
        }
    }
}
