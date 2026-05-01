using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposGasto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposGasto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaRegistracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consorcios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Altura = table.Column<int>(type: "int", nullable: false),
                    DiaVencimientoExpensas = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitud = table.Column<double>(type: "float", nullable: true),
                    Longitud = table.Column<double>(type: "float", nullable: true),
                    IdProvincia = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consorcios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consorcios_Provincias_IdProvincia",
                        column: x => x.IdProvincia,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consorcios_Usuarios_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaGasto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnioExpensa = table.Column<int>(type: "int", nullable: false),
                    MesExpensa = table.Column<int>(type: "int", nullable: false),
                    ArchivoComprobante = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdConsorcio = table.Column<int>(type: "int", nullable: false),
                    IdTipoGasto = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_Consorcios_IdConsorcio",
                        column: x => x.IdConsorcio,
                        principalTable: "Consorcios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_TiposGasto_IdTipoGasto",
                        column: x => x.IdTipoGasto,
                        principalTable: "TiposGasto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Usuarios_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NombrePropietario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApellidoPropietario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmailPropietario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Superficie = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "int", nullable: false),
                    IdConsorcio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unidades_Consorcios_IdConsorcio",
                        column: x => x.IdConsorcio,
                        principalTable: "Consorcios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Unidades_Usuarios_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consorcios_IdProvincia",
                table: "Consorcios",
                column: "IdProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_Consorcios_IdUsuarioCreador",
                table: "Consorcios",
                column: "IdUsuarioCreador");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_IdConsorcio",
                table: "Gastos",
                column: "IdConsorcio");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_IdTipoGasto",
                table: "Gastos",
                column: "IdTipoGasto");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_IdUsuarioCreador",
                table: "Gastos",
                column: "IdUsuarioCreador");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_IdConsorcio",
                table: "Unidades",
                column: "IdConsorcio");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_IdUsuarioCreador",
                table: "Unidades",
                column: "IdUsuarioCreador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "TiposGasto");

            migrationBuilder.DropTable(
                name: "Consorcios");

            migrationBuilder.DropTable(
                name: "Provincias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
