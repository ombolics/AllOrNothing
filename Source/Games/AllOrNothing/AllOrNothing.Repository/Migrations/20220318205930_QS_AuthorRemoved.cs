using Microsoft.EntityFrameworkCore.Migrations;

namespace AllOrNothing.Repository.Migrations
{
    public partial class QS_AuthorRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competences_QuestionSeries_QuestionSerieId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_QuestionSeries_QuestionSerieId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_QuestionSerieId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Competences_QuestionSerieId",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "QuestionSerieId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "QuestionSerieId",
                table: "Competences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionSerieId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionSerieId",
                table: "Competences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_QuestionSerieId",
                table: "Players",
                column: "QuestionSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_Competences_QuestionSerieId",
                table: "Competences",
                column: "QuestionSerieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_QuestionSeries_QuestionSerieId",
                table: "Competences",
                column: "QuestionSerieId",
                principalTable: "QuestionSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_QuestionSeries_QuestionSerieId",
                table: "Players",
                column: "QuestionSerieId",
                principalTable: "QuestionSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
