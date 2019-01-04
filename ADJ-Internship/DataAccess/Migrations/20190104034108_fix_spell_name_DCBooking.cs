using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class fix_spell_name_DCBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DítributionCenter",
                table: "DCBookings",
                newName: "DistributionCenter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistributionCenter",
                table: "DCBookings",
                newName: "DítributionCenter");
        }
    }
}
