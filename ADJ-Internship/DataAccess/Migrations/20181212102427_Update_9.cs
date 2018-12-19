using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class Update_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CAs");

            migrationBuilder.AlterColumn<float>(
                name: "EstQtyToShip",
                table: "ProgressChecks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ReviseQuantity",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviseQuantity",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "EstQtyToShip",
                table: "ProgressChecks",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CAs",
                maxLength: 20,
                nullable: true);
        }
    }
}
