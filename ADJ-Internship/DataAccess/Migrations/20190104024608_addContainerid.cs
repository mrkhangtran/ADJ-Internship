using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class addContainerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContainerId",
                table: "ArriveOfDepacths",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArriveOfDepacths_ContainerId",
                table: "ArriveOfDepacths",
                column: "ContainerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArriveOfDepacths_Container_ContainerId",
                table: "ArriveOfDepacths",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArriveOfDepacths_Container_ContainerId",
                table: "ArriveOfDepacths");

            migrationBuilder.DropIndex(
                name: "IX_ArriveOfDepacths_ContainerId",
                table: "ArriveOfDepacths");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "ArriveOfDepacths");
        }
    }
}
