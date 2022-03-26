using Microsoft.EntityFrameworkCore.Migrations;

namespace AllOrNothing.Repository.Migrations
{
    public partial class Dbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Topic_TopicId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Players_AuthorId",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_QuestionSeries_QuestionSerieId",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_Topic_QuestionSerieId",
                table: "Topics",
                newName: "IX_Topics_QuestionSerieId");

            migrationBuilder.RenameIndex(
                name: "IX_Topic_AuthorId",
                table: "Topics",
                newName: "IX_Topics_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_TopicId",
                table: "Questions",
                newName: "IX_Questions_TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Topics_TopicId",
                table: "Competences",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Players_AuthorId",
                table: "Topics",
                column: "AuthorId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_QuestionSeries_QuestionSerieId",
                table: "Topics",
                column: "QuestionSerieId",
                principalTable: "QuestionSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Topics_TopicId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Players_AuthorId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_QuestionSeries_QuestionSerieId",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_QuestionSerieId",
                table: "Topic",
                newName: "IX_Topic_QuestionSerieId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_AuthorId",
                table: "Topic",
                newName: "IX_Topic_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_TopicId",
                table: "Question",
                newName: "IX_Question_TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Topic_TopicId",
                table: "Competences",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Players_AuthorId",
                table: "Topic",
                column: "AuthorId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_QuestionSeries_QuestionSerieId",
                table: "Topic",
                column: "QuestionSerieId",
                principalTable: "QuestionSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
