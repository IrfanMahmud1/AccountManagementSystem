using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Repositories
{
    public class ChartOfAccountRepository : IChartOfAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public ChartOfAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChartOfAccount>> GetAllAsync()
        {
            return await _context.ChartOfAccounts
            .FromSqlRaw("EXEC sp_ManageChartOfAccounts @Action='SELECT'")
            .ToListAsync();
        }
        public async Task AddAsync(ChartOfAccount account)
        {
            var sql = "EXEC sp_ManageChartOfAccounts @Action='INSERT', @Name={0}, @ParentId={1}, @AccountType={2}, @IsActive={3}";
            await _context.Database.ExecuteSqlRawAsync(sql, account.Name, account.ParentId, account.AccountType, account.IsActive);
        }

    }

}
