using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamenAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepositoInicial = table.Column<decimal>(type: "TEXT", nullable: false),
                    TasaInteresAnual = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlazoEnAños = table.Column<int>(type: "INTEGER", nullable: false),
                    BalanceFinal = table.Column<decimal>(type: "TEXT", nullable: false),
                    InteresTotal = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Simulations");
        }
    }
}
