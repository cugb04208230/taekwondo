using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class AccountHeadPic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadPic",
                table: "UserAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "TrainingOrganizationClassHomeworks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "TrainingOrganizationClassHomeworkAnswers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadPic",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "TrainingOrganizationClassHomeworks");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "TrainingOrganizationClassHomeworkAnswers");
        }
    }
}
