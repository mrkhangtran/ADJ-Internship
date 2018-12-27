using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class add_Bookings_to_Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_OrderId",
                table: "Bookings",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Orders_OrderId",
                table: "Bookings",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Orders_OrderId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_OrderId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bookings");
        }
    }
}
