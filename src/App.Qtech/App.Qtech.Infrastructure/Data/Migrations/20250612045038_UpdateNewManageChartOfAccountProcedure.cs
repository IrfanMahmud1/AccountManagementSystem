using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewManageChartOfAccountProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                    ALTER PROCEDURE [dbo].[sp_ManageChartOfAccounts]
                    @Action NVARCHAR(50),       -- 'INSERT', 'UPDATE', 'DELETE', 'SELECT'
                    @Id UNIQUEIDENTIFIER = NULL,
                    @Name NVARCHAR(100) = NULL,
                    @ParentId UNIQUEIDENTIFIER = NULL,
                    @AccountType NVARCHAR(50) = NULL,
                    @IsActive BIT = NULL
                AS
                BEGIN
                    IF @Action = 'Get'
                    BEGIN
                		SELECT * FROM ChartOfAccounts WHERE Id = @Id 
                    END
                	ELSE IF @Action = 'IsExist'
                    BEGIN
                		SELECT COUNT(*) FROM ChartOfAccounts WHERE Name = @Name 
                    END
                	ELSE  IF @Action = 'GetAll'
                    BEGIN
                		IF @Id IS NOT NULL
                			SELECT * FROM ChartOfAccounts WHERE Id != @Id 
                		ELSE
                			SELECT * 
                            FROM ChartOfAccounts
                    END
                	ELSE  IF @Action = 'GetAllWithNoChild'
                    BEGIN
                		SELECT * FROM ChartOfAccounts WHERE Id != @Id AND @Id NOT IN (SELECT DISTINCT ParentId FROM ChartOfAccounts WHERE ParentId IS NOT NULL) OR Id != @Id AND ParentId IS NULL
                    END
                	ELSE  IF @Action = 'GetAllChilds'
                    BEGIN
                		SELECT * FROM ChartOfAccounts WHERE ParentId = @ParentId
                    END
                    ELSE IF @Action = 'Create'
                    BEGIN
                        INSERT INTO ChartOfAccounts (Id,Name, ParentId, AccountType, IsActive, CreatedAt)
                        VALUES (NEWID(),@Name, @ParentId, @AccountType, @IsActive, GETDATE())
                    END
                    ELSE IF @Action = 'Update'
                    BEGIN
                        UPDATE ChartOfAccounts
                        SET Name = @Name, ParentId = @ParentId, AccountType = @AccountType, IsActive = @IsActive
                        WHERE Id = @Id
                    END
                    ELSE IF @Action = 'Delete'
                    BEGIN
                        DELETE FROM ChartOfAccounts WHERE Id = @Id
                    END
                END

                
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_ManageChartOfAccounts]";
            migrationBuilder.Sql(sql);
        }
    }
}
