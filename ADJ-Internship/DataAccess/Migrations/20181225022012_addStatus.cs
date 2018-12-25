using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class addStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Bookings",
                newName: "PortOfLoading");

            migrationBuilder.RenameColumn(
                name: "Item",
                table: "Bookings",
                newName: "PortOfDelivery");

            migrationBuilder.RenameColumn(
                name: "Carier",
                table: "Bookings",
                newName: "PONumber");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Factory",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemNumber",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShipmentID",
                table: "Bookings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Factory",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ItemNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ShipmentID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PortOfLoading",
                table: "Bookings",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "PortOfDelivery",
                table: "Bookings",
                newName: "Item");

            migrationBuilder.RenameColumn(
                name: "PONumber",
                table: "Bookings",
                newName: "Carier");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
