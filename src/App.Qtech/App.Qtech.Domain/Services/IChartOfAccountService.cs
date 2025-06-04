using App.Qtech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Services
{
    public interface IChartOfAccountService
    {
        Task<ChartOfAccount> GetAccountByIdAsync(Guid id);
        Task<List<ChartOfAccount>> GetAllAccountsAsync();
        Task AddAccountAsync(ChartOfAccount account);
        Task UpdateAccountAsync(ChartOfAccount account);
        Task DeleteAccountAsync(Guid id);
    }

}
