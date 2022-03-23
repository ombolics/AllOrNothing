using Microsoft.EntityFrameworkCore.Migrations;

namespace AllOrNothing.Repository.Migrations
{
    public partial class typoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Institue",
                table: "Players",
                newName: "Institute");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Institute",
                table: "Players",
                newName: "Institue");
        }
    }
}
