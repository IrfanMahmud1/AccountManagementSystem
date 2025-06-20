﻿using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using Azure;
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

        public async Task<IList<RoleModuleAccess>> GetAllAsync()
        {
            var roleModuleAccesses = new List<RoleModuleAccess>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageRoleModuleAccess", connection)
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
                    HasAccess = reader.GetBoolean(reader.GetOrdinal("HasAccess")),
                    Operation = reader.GetString(reader.GetOrdinal("Operation"))
                });
            }

            return roleModuleAccesses;
        }

        public async Task<List<string>> GetAsync(string role)
        {
            var modules = new List<string>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageRoleModuleAccess", connection)
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
                if (!reader.IsDBNull(0))
                {
                    modules.Add(reader.GetString(0));
                }
            }

            return modules;
        }



        private async Task<bool> ExecuteNonQueryAsync(RoleModuleAccess roleModuleAccess, RoleModuleAccessAction operation)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageRoleModuleAccess", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", operation.ToString());
            command.Parameters.AddWithValue("@Id", 
                roleModuleAccess.Id == Guid.Empty ? DBNull.Value : roleModuleAccess.Id);

            if (operation is RoleModuleAccessAction.Create or RoleModuleAccessAction.Update)
            {
                command.Parameters.AddWithValue("@RoleName", 
                    roleModuleAccess.RoleName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModuleName", 
                    roleModuleAccess.ModuleName ?? (object)DBNull.Value);

                command.Parameters.AddWithValue("@HasAccess", roleModuleAccess.HasAccess);

                command.Parameters.AddWithValue("@Operation", 
                    roleModuleAccess.Operation ?? (object)DBNull.Value);
            }

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> HasAcess(string role, string module,string operation)
        {
            int count = 0;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageRoleModuleAccess", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", RoleModuleAccessAction.CheckAccess.ToString());
            command.Parameters.AddWithValue("@Id", DBNull.Value);
            command.Parameters.AddWithValue("@RoleName", string.IsNullOrEmpty(role) 
                ? (object)DBNull.Value : role);
            command.Parameters.AddWithValue("@ModuleName", string.IsNullOrEmpty(module) 
                ? (object)DBNull.Value : module);
            command.Parameters.AddWithValue("@Operation", string.IsNullOrEmpty(operation)
                ? (object)DBNull.Value : operation);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            if (result != null && result != DBNull.Value)
            {
                count = Convert.ToInt32(result);
            }
            return count > 0;
        }

        public async Task<RoleModuleAccess> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageRoleModuleAccess", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", RoleModuleAccessAction.GetById.ToString());
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new RoleModuleAccess
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                    Operation = reader.GetString(reader.GetOrdinal("Operation")),
                    HasAccess = reader.GetBoolean(reader.GetOrdinal("HasAccess"))
                };
            }

            return null;
        }
    }
    public enum RoleModuleAccessAction
    {
        Get,
        GetById,
        GetAll,
        Create,
        Update,
        Delete,
        CheckAccess
    }
}

