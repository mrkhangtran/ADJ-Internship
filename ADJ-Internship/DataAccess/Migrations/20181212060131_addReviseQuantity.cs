using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class addReviseQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "EstQtyToShip",
                table: "ProgressChecks",
                nullable: false,
                oldClrType: typeof(int));

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
        }
    }
}
