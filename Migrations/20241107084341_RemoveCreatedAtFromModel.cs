using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class RemoveCreatedAtFromModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP"); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }
    }

}
