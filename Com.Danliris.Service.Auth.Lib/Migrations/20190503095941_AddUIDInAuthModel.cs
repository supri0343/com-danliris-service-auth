using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Auth.Lib.Migrations
{
    public partial class AddUIDInAuthModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Roles",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Permissions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "Accounts",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "AccountRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "AccountProfiles",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "AccountProfiles");
        }
    }
}
