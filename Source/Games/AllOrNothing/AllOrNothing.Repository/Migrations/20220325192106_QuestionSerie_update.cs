using Microsoft.EntityFrameworkCore.Migrations;

namespace AllOrNothing.Repository.Migrations
{
    public partial class QuestionSerie_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionSeries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "QuestionSeries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionSeries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "QuestionSeries");
        }
    }
}
