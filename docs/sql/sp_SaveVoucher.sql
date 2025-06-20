USE [AppQtec]
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveVoucher]    Script Date: 6/9/2025 11:55:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[sp_SaveVoucher]
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
