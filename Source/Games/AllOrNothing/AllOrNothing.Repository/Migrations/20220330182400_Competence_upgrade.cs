using Microsoft.EntityFrameworkCore.Migrations;

namespace AllOrNothing.Repository.Migrations
{
    public partial class Competence_upgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Topics_TopicId",
                table: "Competences");

            migrationBuilder.DropIndex(
                name: "IX_Competences_TopicId",
                table: "Competences");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Competences");

            migrationBuilder.CreateTable(
                name: "CompetenceTopic",
                columns: table => new
                {
                    CompetencesId = table.Column<int>(type: "int", nullable: false),
                    TopicsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenceTopic", x => new { x.CompetencesId, x.TopicsId });
                    table.ForeignKey(
                        name: "FK_CompetenceTopic_Competences_CompetencesId",
                        column: x => x.CompetencesId,
                        principalTable: "Competences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetenceTopic_Topics_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetenceTopic_TopicsId",
                table: "CompetenceTopic",
                column: "TopicsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenceTopic");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Competences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competences_TopicId",
                table: "Competences",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Topics_TopicId",
                table: "Competences",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
