using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class HomeAnswerFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Video",
                table: "TrainingOrganizationClassHomeworks",
                newName: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Files",
                table: "TrainingOrganizationClassHomeworks",
                newName: "Video");
        }
    }
}
