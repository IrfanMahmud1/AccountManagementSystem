using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewOperationColumnToRoleModuleAccessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "RoleModuleAccesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operation",
                table: "RoleModuleAccesses");
        }
    }
}
