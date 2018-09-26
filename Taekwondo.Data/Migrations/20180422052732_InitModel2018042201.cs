using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class InitModel2018042201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dan",
                table: "TrainingOrganizationClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dan",
                table: "GenearchChildren",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dan",
                table: "TrainingOrganizationClasses");

            migrationBuilder.DropColumn(
                name: "Dan",
                table: "GenearchChildren");
        }
    }
}
