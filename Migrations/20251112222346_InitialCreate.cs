using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrupoCeleste.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Esta migración está vacía porque las tablas ya existen en la base de datos
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No hacer nada en el rollback
        }
    }
}
