using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class InitModel2018050301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenearchChild",
                table: "TrainingOrganizationClassStudentLessonMaps",
                newName: "GenearchChildId");

            migrationBuilder.RenameColumn(
                name: "GenearchChild",
                table: "TrainingOrganizationClassStudentLessonLeaves",
                newName: "GenearchChildId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAgreed",
                table: "TrainingOrganizationClassStudentLessonLeaves",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenearchChildId",
                table: "TrainingOrganizationClassStudentLessonMaps",
                newName: "GenearchChild");

            migrationBuilder.RenameColumn(
                name: "GenearchChildId",
                table: "TrainingOrganizationClassStudentLessonLeaves",
                newName: "GenearchChild");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAgreed",
                table: "TrainingOrganizationClassStudentLessonLeaves",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
