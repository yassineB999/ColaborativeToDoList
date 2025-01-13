using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeToDoList.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_UsersId",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_UsersId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "TodoLists");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "TodoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_OwnerId",
                table: "TodoLists",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_OwnerId",
                table: "TodoLists",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_OwnerId",
                table: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_OwnerId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TodoLists");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "TodoLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_UsersId",
                table: "TodoLists",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_UsersId",
                table: "TodoLists",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
