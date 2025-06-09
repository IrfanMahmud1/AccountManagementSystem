CREATE TYPE dbo.VoucherEntryTableType AS TABLE
(
    EntryId UNIQUEIDENTIFIER,
    VoucherId UNIQUEIDENTIFIER,
    AccountName NVARCHAR(100),
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
);
