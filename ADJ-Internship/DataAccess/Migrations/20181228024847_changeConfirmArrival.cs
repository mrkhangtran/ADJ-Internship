using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class changeConfirmArrival : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAs_Bookings_BookingId",
                table: "CAs");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "CAs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ContainerId",
                table: "CAs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs",
                column: "ContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CAs_Bookings_BookingId",
                table: "CAs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CAs_Container_ContainerId",
                table: "CAs",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAs_Bookings_BookingId",
                table: "CAs");

            migrationBuilder.DropForeignKey(
                name: "FK_CAs_Container_ContainerId",
                table: "CAs");

            migrationBuilder.DropIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "CAs");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "CAs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CAs_Bookings_BookingId",
                table: "CAs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
