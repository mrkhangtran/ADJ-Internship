using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class addStatus_updateShipmentTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cartoons",
                table: "Bookings");

            migrationBuilder.AlterColumn<float>(
                name: "KGS",
                table: "Manifests",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "Cube",
                table: "Manifests",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "Cartons",
                table: "Manifests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Cube",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<float>(
                name: "Cartons",
                table: "Bookings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cartons",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "KGS",
                table: "Manifests",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "Cube",
                table: "Manifests",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Cartons",
                table: "Manifests",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "Cube",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<int>(
                name: "Cartoons",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
