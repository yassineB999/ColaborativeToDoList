using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeToDoList.Migrations
{
    /// <inheritdoc />
    public partial class updateMigrationv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_OwnerId",
                table: "TodoLists");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "TodoLists",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_OwnerId",
                table: "TodoLists",
                newName: "IX_TodoLists_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_UsersId",
                table: "TodoLists",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_UsersId",
                table: "TodoLists");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "TodoLists",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_UsersId",
                table: "TodoLists",
                newName: "IX_TodoLists_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_OwnerId",
                table: "TodoLists",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
