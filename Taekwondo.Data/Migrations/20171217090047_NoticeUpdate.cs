using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class NoticeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingOrganizationClassHomeworkMarkings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    GenearchChildId = table.Column<long>(nullable: false),
                    GenearchId = table.Column<long>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    TeacherId = table.Column<long>(nullable: false),
                    TrainingOrganizationClassHomeworkId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingOrganizationClassHomeworkMarkings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "TrainingOrganizationClassHomeworkMarkings");
        }
    }
}
