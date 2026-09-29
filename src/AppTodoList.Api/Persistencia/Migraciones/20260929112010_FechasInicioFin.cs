using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppTodoList.Api.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class FechasInicioFin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaFin",
                table: "Tareas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaInicio",
                table: "Tareas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Tareas");
        }
    }
}
