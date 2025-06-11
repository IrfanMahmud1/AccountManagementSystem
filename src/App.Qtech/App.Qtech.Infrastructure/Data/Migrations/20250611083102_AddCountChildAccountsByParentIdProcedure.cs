using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCountChildAccountsByParentIdProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                CREATE OR ALTER PROCEDURE [dbo].[sp_countChildAccountsByParentId]
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT COUNT(*) AS ChildCount
                    FROM ChartOfAccounts
                    WHERE ParentId = @Id;
                END;
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_countChildAccountsByParentId]";
            migrationBuilder.Sql(sql);
        }
    }
}
