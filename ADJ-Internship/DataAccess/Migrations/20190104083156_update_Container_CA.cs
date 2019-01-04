using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class update_Container_CA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs");

            migrationBuilder.CreateIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs",
                column: "ContainerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs");

            migrationBuilder.CreateIndex(
                name: "IX_CAs_ContainerId",
                table: "CAs",
                column: "ContainerId");
        }
    }
}
