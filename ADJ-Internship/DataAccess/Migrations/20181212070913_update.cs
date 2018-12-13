using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgressChecks_OrderId",
                table: "ProgressChecks");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProgressChecks_OrderId",
                table: "ProgressChecks",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgressChecks_OrderId",
                table: "ProgressChecks");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProgressChecks_OrderId",
                table: "ProgressChecks",
                column: "OrderId");
        }
    }
}
