﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeToDoList.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsApprover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Collaborators",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Collaborators");
        }
    }
}
