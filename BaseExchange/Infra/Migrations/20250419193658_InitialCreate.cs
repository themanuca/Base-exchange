using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExposicaoFinanceira",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExposicaoFinanceira", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ordem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lado = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Aceita = table.Column<bool>(type: "bit", nullable: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordem", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExposicaoFinanceira",
                columns: new[] { "Id", "Ativo", "AtualizadoEm", "Valor" },
                values: new object[,]
                {
                    { 1, "PETR4", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), 0m },
                    { 2, "VALE3", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), 0m },
                    { 3, "VIIA4", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExposicaoFinanceira");

            migrationBuilder.DropTable(
                name: "Ordem");
        }
    }
}
