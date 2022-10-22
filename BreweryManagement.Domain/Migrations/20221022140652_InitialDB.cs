using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreweryManagement.Domain.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brewers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brewers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WholeSalers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholeSalers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AlcoholContent = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    BrewerId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beers_Brewers_BrewerId",
                        column: x => x.BrewerId,
                        principalTable: "Brewers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholeSalerBeers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WholeSalerId = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    BeerId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholeSalerBeers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholeSalerBeers_Beers_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WholeSalerBeers_WholeSalers_WholeSalerId",
                        column: x => x.WholeSalerId,
                        principalTable: "WholeSalers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BrewerId",
                table: "Beers",
                column: "BrewerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholeSalerBeers_BeerId",
                table: "WholeSalerBeers",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholeSalerBeers_WholeSalerId",
                table: "WholeSalerBeers",
                column: "WholeSalerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WholeSalerBeers");

            migrationBuilder.DropTable(
                name: "Beers");

            migrationBuilder.DropTable(
                name: "WholeSalers");

            migrationBuilder.DropTable(
                name: "Brewers");
        }
    }
}
