using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Qtech.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManageRoleModuleAccessProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                CREATE PROCEDURE [dbo].[sp_ManageRoleModuleAccess]
                    @Action NVARCHAR(50),       -- 'INSERT', 'UPDATE', 'DELETE', 'SELECT'
                    @Id UNIQUEIDENTIFIER = NULL,
                    @RoleName NVARCHAR(100) = NULL,
                	@ModuleName NVARCHAR(100) = NULL,
                    @HasAccess BIT = NULL,
                	@Operation NVARCHAR(50) = NULL
                AS
                BEGIN
                    IF @Action = 'Get'
                    BEGIN
                		IF @RoleName IS NOT NULL
                			SELECT @ModuleName FROM RoleModuleAccesses WHERE RoleName = @RoleName
                    END
                	ELSE IF @Action = 'GetAll'
                    BEGIN
                		SELECT * FROM RoleModuleAccesses
                    END
                	ELSE IF @Action = 'GetById'
                    BEGIN
                		SELECT * FROM RoleModuleAccesses WHERE Id = @Id
                    END
                	ELSE IF @Action = 'CheckAccess'
                    BEGIN
                		SELECT COUNT(*) 
                		FROM RoleModuleAccesses
                		WHERE RoleName = @RoleName AND ModuleName = @ModuleName AND Operation = @Operation
                    END
                    ELSE IF @Action = 'Create'
                    BEGIN
                        INSERT INTO RoleModuleAccesses (Id,RoleName, ModuleName,HasAccess,Operation)
                        VALUES (NEWID(),@RoleName, @ModuleName, @HasAccess,@Operation)
                    END
                    ELSE IF @Action = 'Update'
                    BEGIN
                        UPDATE RoleModuleAccesses
                        SET RoleName = @RoleName, ModuleName= @ModuleName, HasAccess = @HasAccess
                        WHERE Id = @Id
                    END
                    ELSE IF @Action = 'Delete'
                    BEGIN
                        DELETE FROM RoleModuleAccesses WHERE Id = @Id
                    END
                END
                
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_ManageRoleModuleAccess]";
            migrationBuilder.Sql(sql);
        }
    }
}
