using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Auth.Lib.Migrations
{
    public partial class enhanceroleformenuAcces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission2_Accounts_AccountId",
                table: "Permission2");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Permission2",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission2_AccountId",
                table: "Permission2",
                newName: "IX_Permission2_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission2_Roles_RoleId",
                table: "Permission2",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission2_Roles_RoleId",
                table: "Permission2");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Permission2",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission2_RoleId",
                table: "Permission2",
                newName: "IX_Permission2_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission2_Accounts_AccountId",
                table: "Permission2",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
