using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Repositories
{
    //public class ChartOfAccountRepository(IConfiguration configration) : IChartOfAccountRepository
    //{
    //    private readonly string _connection = configration.GetConnectionString("DefaultConnection");

    //    public async Task<List<ChartOfAccount>> GetAllAsync()
    //    {
    //        var parameters = new Dictionary<string, object>
    //        {
    //            { "Action","SELECT" }
    //        };
    //        var command = CreateCommand("sp_ManageChartOfAccounts", parameters);
    //        var reader = await command.ExecuteReaderAsync();
    //        var result = new List<ChartOfAccount>();
    //        while(await reader.ReadAsync())
    //        {
    //            for (int i = 0; i < reader.FieldCount; i++)
    //            {
    //                var account = new ChartOfAccount
    //                {
    //                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
    //                    Name = reader.GetString(reader.GetOrdinal("Name")),
    //                    ParentId = reader.GetGuid(reader.GetOrdinal("ParentId")),
    //                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
    //                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
    //                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
    //                };
    //                result.Add(account);
    //            }
    //        }
    //        return result;
    //    }
    //    public async Task AddAsync(ChartOfAccount account)
    //    {

    //    }
    //    private DbCommand CreateCommand(string storedProcedureName,
    //        IDictionary<string, object> parameters = null)
    //    {
    //        var command = new SqlCommand(_connection);
    //        command.CommandText = storedProcedureName;
    //        command.CommandType = CommandType.StoredProcedure;

    //        if (parameters != null)
    //        {
    //            foreach (var item in parameters)
    //            {
    //                command.Parameters.Add(new SqlParameter(item.Key, item.Value));
    //            }
    //        }

    //        return command;
    //    }

    //}
    public class ChartOfAccountRepository : IChartOfAccountRepository
    {
        private readonly string _connectionString;

        public ChartOfAccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ChartOfAccount?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageChartOfAccounts", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", ChartOfAccountAction.Get.ToString());
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ChartOfAccount
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                };
            }

            return null;
        }
        public async Task<List<ChartOfAccount>> GetAllAsync(Guid? id = null)
        {
            var accounts = new List<ChartOfAccount>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageChartOfAccounts", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Action", ChartOfAccountAction.GetAll.ToString());
            command.Parameters.AddWithValue("@Id", id == null ? DBNull.Value : id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                accounts.Add(new ChartOfAccount
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return accounts;
        }

        public async Task<List<ChartOfAccount>> GetAllWithNoChildAsync(Guid id)
        {
            var accounts = new List<ChartOfAccount>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageChartOfAccounts", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Action", ChartOfAccountAction.GetAllWithNoChild.ToString());
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                accounts.Add(new ChartOfAccount
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return accounts;
        }
        public async Task<List<ChartOfAccount>> GetAllChildsAsync(Guid id)
        {
            var accounts = new List<ChartOfAccount>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageChartOfAccounts", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@Action", ChartOfAccountAction.GetAllChilds.ToString());
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                accounts.Add(new ChartOfAccount
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return accounts;
        }
        public async Task<bool> CreateAsync(ChartOfAccount account)
        {
            return await ExecuteNonQuery(account, ChartOfAccountAction.Create);
        }

        public async Task<bool> UpdateAsync(ChartOfAccount account)
        {
            return await ExecuteNonQuery(account, ChartOfAccountAction.Update);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var account = new ChartOfAccount { Id = id };
            return await ExecuteNonQuery(account, ChartOfAccountAction.Delete);
        }
        public async Task<int> GetChildCountByParentIdAsync(Guid id)
        {
            int count = 0;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_countChildAccountsByParentId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    count = Convert.ToInt32(result);
                }
            }

            return count;
        }

        private async Task<bool> ExecuteNonQuery(ChartOfAccount account, ChartOfAccountAction operation)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ManageChartOfAccounts", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Action", operation.ToString());
            command.Parameters.AddWithValue("@Id", account.Id);

            if (operation is ChartOfAccountAction.Create or ChartOfAccountAction.Update)
            {
                command.Parameters.AddWithValue("@Name", account.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ParentId", account.ParentId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AccountType", account.AccountType ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", account.IsActive);
            }

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
    public enum ChartOfAccountAction
    {
        Get,
        GetAll,
        GetAllWithNoChild,
        GetAllChilds,
        Create,
        Update,
        Delete
    }

}
