using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineService.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.RenameTable(
                name: "Flight",
                newName: "Flights");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    ConfirmationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => new { x.FlightId, x.PassengerId });
                    table.ForeignKey(
                        name: "FK_Booking_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PassengerId",
                table: "Booking",
                column: "PassengerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "Flight");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                column: "Id");
        }
    }
}
