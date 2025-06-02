CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10),       -- 'INSERT', 'UPDATE', 'DELETE', 'SELECT'
    @Id uniqueidentifier = NULL,
    @Name NVARCHAR(100) = NULL,
    @ParentId INT = NULL,
    @AccountType NVARCHAR(50) = NULL,
    @IsActive BIT = NULL
AS
BEGIN
    IF @Action = 'SELECT'
    BEGIN
        SELECT * FROM ChartOfAccounts
    END
    ELSE IF @Action = 'INSERT'
    BEGIN
        INSERT INTO ChartOfAccounts (Name, ParentId, AccountType, IsActive, CreatedAt)
        VALUES (@Name, @ParentId, @AccountType, @IsActive, GETDATE())
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE ChartOfAccounts
        SET Name = @Name, ParentId = @ParentId, AccountType = @AccountType, IsActive = @IsActive
        WHERE Id = @Id
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM ChartOfAccounts WHERE Id = @Id
    END
END
