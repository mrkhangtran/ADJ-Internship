using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class update_Container : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings");

            migrationBuilder.CreateIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations",
                column: "ContainerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings",
                column: "ContainerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings");

            migrationBuilder.CreateIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings",
                column: "ContainerId");
        }
    }
}
