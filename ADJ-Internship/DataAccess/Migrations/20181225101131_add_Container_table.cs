using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class add_Container_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Container",
                table: "Manifests");

            migrationBuilder.RenameColumn(
                name: "Cartoons",
                table: "Manifests",
                newName: "ContainerId");

            migrationBuilder.CreateTable(
                name: "Container",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Loading = table.Column<string>(nullable: true),
                    PackType = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Container", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_ContainerId",
                table: "Manifests",
                column: "ContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manifests_Container_ContainerId",
                table: "Manifests",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manifests_Container_ContainerId",
                table: "Manifests");

            migrationBuilder.DropTable(
                name: "Container");

            migrationBuilder.DropIndex(
                name: "IX_Manifests_ContainerId",
                table: "Manifests");

            migrationBuilder.RenameColumn(
                name: "ContainerId",
                table: "Manifests",
                newName: "Cartoons");

            migrationBuilder.AddColumn<string>(
                name: "Container",
                table: "Manifests",
                nullable: true);
        }
    }
}
