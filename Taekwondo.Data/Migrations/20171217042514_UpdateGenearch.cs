using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class UpdateGenearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurriculumVitae",
                table: "TrainingOrganizationTeachers");

            migrationBuilder.RenameColumn(
                name: "TrainingOrganizationSubjectId",
                table: "TrainingOrganizationTeachers",
                newName: "UserId");

            migrationBuilder.AddColumn<long>(
                name: "GenearchId",
                table: "GenearchChildren",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenearchId",
                table: "GenearchChildren");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TrainingOrganizationTeachers",
                newName: "TrainingOrganizationSubjectId");

            migrationBuilder.AddColumn<string>(
                name: "CurriculumVitae",
                table: "TrainingOrganizationTeachers",
                nullable: true);
        }
    }
}
