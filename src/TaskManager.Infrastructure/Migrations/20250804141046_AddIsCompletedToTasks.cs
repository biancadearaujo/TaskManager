using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "TaskEntity",
                newName: "IX_TaskEntity_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskEntity",
                table: "TaskEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEntity_AspNetUsers_UserId",
                table: "TaskEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskEntity_AspNetUsers_UserId",
                table: "TaskEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskEntity",
                table: "TaskEntity");

            migrationBuilder.RenameTable(
                name: "TaskEntity",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskEntity_UserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
