using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiUdemy.Migrations
{
    public partial class FechaPublicacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPubligacion",
                table: "Libros",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaPubligacion",
                table: "Libros");
        }
    }
}
