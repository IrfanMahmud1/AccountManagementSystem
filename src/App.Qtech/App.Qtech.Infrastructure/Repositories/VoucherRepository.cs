using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly string? _connectionString;

        public VoucherRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<VoucherDisplayDto>> GetAllDisplaysAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM vw_VoucherDetails", conn);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            var VoucherRows = new List<VoucherDisplayDto>();
            while (await reader.ReadAsync())
            {
                VoucherRows.Add(new VoucherDisplayDto
                {
                    Date = reader.GetDateTime(0),
                    ReferenceNo = reader.GetString(1),
                    Type = reader.GetString(2).ToString(), // Enum to string
                    AccountName = reader.GetString(3),
                    Debit = reader.GetDecimal(4),
                    Credit = reader.GetDecimal(5)
                });
            }
            return VoucherRows;
        }

        public async Task SaveAsync(Voucher voucher)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_SaveVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VoucherId", voucher.Id);
            cmd.Parameters.AddWithValue("@Date", voucher.Date);
            cmd.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
            cmd.Parameters.AddWithValue("@Type", voucher.Type);

            var entries = new DataTable();
            entries.Columns.Add("Id", typeof(Guid));
            entries.Columns.Add("VoucherId", typeof(Guid));
            entries.Columns.Add("AccountName", typeof(string));
            entries.Columns.Add("Debit", typeof(decimal));
            entries.Columns.Add("Credit", typeof(decimal));

            foreach (var e in voucher.Entries)
            {
                entries.Rows.Add(e.Id, voucher.Id, e.AccountName, e.Debit, e.Credit);
            }

            var entriesParam = cmd.Parameters.AddWithValue("@Entries", entries);
            entriesParam.SqlDbType = SqlDbType.Structured;
            entriesParam.TypeName = "dbo.VoucherEntryTableType"; // Define this TVP in DB

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }

}
