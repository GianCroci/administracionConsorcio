using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SumsConReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservaSum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSum = table.Column<int>(type: "int", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Turno = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EntregoCorrectamente = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaSum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservaSum_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdConsorcio = table.Column<int>(type: "int", nullable: false),
                    ReservaSumId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sum_Consorcios_IdConsorcio",
                        column: x => x.IdConsorcio,
                        principalTable: "Consorcios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sum_ReservaSum_ReservaSumId",
                        column: x => x.ReservaSumId,
                        principalTable: "ReservaSum",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaSum_IdSum",
                table: "ReservaSum",
                column: "IdSum");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaSum_IdUsuario",
                table: "ReservaSum",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Sum_IdConsorcio",
                table: "Sum",
                column: "IdConsorcio");

            migrationBuilder.CreateIndex(
                name: "IX_Sum_ReservaSumId",
                table: "Sum",
                column: "ReservaSumId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaSum_Sum_IdSum",
                table: "ReservaSum",
                column: "IdSum",
                principalTable: "Sum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaSum_Sum_IdSum",
                table: "ReservaSum");

            migrationBuilder.DropTable(
                name: "Sum");

            migrationBuilder.DropTable(
                name: "ReservaSum");
        }
    }
}
