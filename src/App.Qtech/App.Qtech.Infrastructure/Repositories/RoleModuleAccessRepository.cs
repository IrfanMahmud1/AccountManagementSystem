using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Repositories
{
    public class RoleModuleAccessRepository : IRoleModuleAccessRepository
    {
        private readonly string? _connectionString;

        public RoleModuleAccessRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> AddAsync(RoleModuleAccess moduleAccess)
        {
            return await ExecuteNonQueryAsync(moduleAccess,RoleModuleAccessAction.Create);
        }

        public async Task<bool> UpdateAsync(RoleModuleAccess moduleAccess)
        {
            return await ExecuteNonQueryAsync(moduleAccess, RoleModuleAccessAction.Update);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var roleModuleAccess = new RoleModuleAccess { Id = id };
            return await ExecuteNonQueryAsync(roleModuleAccess, RoleModuleAccessAction.Delete);
        }

        public async Task<IEnumerable<RoleModuleAccess>> GetAllAsync()
        {
            var roleModuleAccesses = new List<RoleModuleAccess>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Action", RoleModuleAccessAction.GetAll.ToString());

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roleModuleAccesses.Add(new RoleModuleAccess
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                    HasAccess = reader.GetBoolean(reader.GetOrdinal("HasAccess"))
                });
            }

            return roleModuleAccesses;
        }

        public async Task<List<string>> GetAsync(string role)
        {
            var modules = new List<string>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Action", RoleModuleAccessAction.Get.ToString());
            command.Parameters.AddWithValue("@RoleName", 
                string.IsNullOrEmpty(role) ? (object)DBNull.Value : role);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                modules.Add(reader.GetString(0));
            }

            return modules;
        }



        private async Task<bool> ExecuteNonQueryAsync(RoleModuleAccess roleModuleAccess, RoleModuleAccessAction operation)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", operation.ToString());
            command.Parameters.AddWithValue("@Id", 
                roleModuleAccess.Id == Guid.Empty ? DBNull.Value : roleModuleAccess.Id);

            if (operation is RoleModuleAccessAction.Create or RoleModuleAccessAction.Update)
            {
                command.Parameters.AddWithValue("@RoleName", roleModuleAccess.RoleName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModuleName", roleModuleAccess.ModuleName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@HasAccess", roleModuleAccess.HasAccess);
            }

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
    public enum RoleModuleAccessAction
    {
        Get,
        GetAll,
        Create,
        Update,
        Delete
    }
}

