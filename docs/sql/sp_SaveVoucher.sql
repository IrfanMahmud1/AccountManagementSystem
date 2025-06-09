CREATE OR ALTER PROCEDURE sp_SaveVoucher
    @VoucherId UNIQUEIDENTIFIER,
    @Date DATE,
    @ReferenceNo NVARCHAR(50),
    @Type INT,
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
