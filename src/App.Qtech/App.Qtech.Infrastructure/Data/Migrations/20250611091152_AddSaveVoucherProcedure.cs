using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSaveVoucherProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                CREATE PROCEDURE [dbo].[sp_SaveVoucher]
                    @VoucherId UNIQUEIDENTIFIER,
                    @Date DATE,
                    @ReferenceNo NVARCHAR(50),
                    @Type NVARCHAR(10),
                    @Entries dbo.VoucherEntryTableType READONLY
                AS
                BEGIN
                    BEGIN TRY
                        BEGIN TRANSACTION;

                        -- Insert into Vouchers table
                        INSERT INTO Vouchers (Id, Date, ReferenceNo, Type)
                        VALUES (@VoucherId, @Date, @ReferenceNo, @Type);

                        -- Insert into VoucherEntries table
                        INSERT INTO VoucherEntries (Id, VoucherId, AccountName, Debit, Credit)
                        SELECT EntryId, VoucherId, AccountName, Debit, Credit
                        FROM @Entries;

                        COMMIT;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK;
                        THROW;
                    END CATCH
                END
                
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_SaveVoucher]";
            migrationBuilder.Sql(sql);
        }
    }
}
