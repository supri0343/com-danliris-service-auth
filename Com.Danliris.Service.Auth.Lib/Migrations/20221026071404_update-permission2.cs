using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Auth.Lib.Migrations
{
    public partial class updatepermission2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Permission2");

            migrationBuilder.RenameColumn(
                name: "MenuCode",
                table: "Permission2",
                newName: "Menu");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Permission2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Permission2");

            migrationBuilder.RenameColumn(
                name: "Menu",
                table: "Permission2",
                newName: "MenuCode");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Permission2",
                nullable: false,
                defaultValue: 0);
        }
    }
}
