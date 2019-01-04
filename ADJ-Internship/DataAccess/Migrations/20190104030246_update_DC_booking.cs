using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class update_DC_booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "DCBookings");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryTime",
                table: "DCConfirmations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContainerId",
                table: "DCConfirmations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "WareHouse",
                table: "DCBookings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Haulier",
                table: "DCBookings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingTime",
                table: "DCBookings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingRef",
                table: "DCBookings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "DCBookings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContainerId",
                table: "DCBookings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DítributionCenter",
                table: "DCBookings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings",
                column: "ContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DCBookings_Container_ContainerId",
                table: "DCBookings",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DCConfirmations_Container_ContainerId",
                table: "DCConfirmations",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DCBookings_Container_ContainerId",
                table: "DCBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DCConfirmations_Container_ContainerId",
                table: "DCConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_DCConfirmations_ContainerId",
                table: "DCConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_DCBookings_ContainerId",
                table: "DCBookings");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "DCConfirmations");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "DCBookings");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "DCBookings");

            migrationBuilder.DropColumn(
                name: "DítributionCenter",
                table: "DCBookings");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryTime",
                table: "DCConfirmations",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WareHouse",
                table: "DCBookings",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Haulier",
                table: "DCBookings",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingTime",
                table: "DCBookings",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingRef",
                table: "DCBookings",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                table: "DCBookings",
                maxLength: 20,
                nullable: true);
        }
    }
}
