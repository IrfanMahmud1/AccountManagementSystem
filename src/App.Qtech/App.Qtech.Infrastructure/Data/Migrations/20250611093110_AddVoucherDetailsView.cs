using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                CREATE   VIEW [dbo].[vw_VoucherDetails] AS
                SELECT 
                    v.Date,
                    v.ReferenceNo,
                    v.Type,
                    ve.AccountName,
                    ve.Debit,
                    ve.Credit
                FROM Vouchers v
                JOIN VoucherEntries ve ON v.Id = ve.VoucherId;
                GO
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP VIEW [dbo].[vw_VoucherDetails]";
            migrationBuilder.Sql(sql);
        }
    }
}
