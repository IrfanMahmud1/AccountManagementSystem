CREATE OR ALTER VIEW vw_VoucherDetails AS
SELECT 
    v.Date,
    v.ReferenceNo,
    v.Type,
    ve.AccountName,
    ve.Debit,
    ve.Credit
FROM Vouchers v
JOIN VoucherEntries ve ON v.Id = ve.VoucherId;
