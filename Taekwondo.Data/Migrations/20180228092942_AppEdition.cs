using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taekwondo.Data.Migrations
{
    public partial class AppEdition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppEditions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false, defaultValueSql: "next value for Ids"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Edition = table.Column<string>(nullable: true),
                    IsMandatory = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    Os = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEditions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppEditions");
        }
    }
}
