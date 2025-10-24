using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrupoCeleste.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Asunto = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MensajeTexto = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Leido = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    FechaLectura = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Email",
                table: "Mensajes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Fecha",
                table: "Mensajes",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Leido",
                table: "Mensajes",
                column: "Leido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensajes");
        }
    }
}
