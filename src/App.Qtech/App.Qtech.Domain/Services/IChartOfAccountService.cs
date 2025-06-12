using App.Qtech.Domain.Entities;
using App.Qtech.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Services
{
    public interface IChartOfAccountService
    {
        Task<PagedResult<ChartOfAccount>> GetPaginatedChartOfAccounts(int pageNumber,int pageSize);
        Task<bool> IsDuplicate(string name);
        Task<ChartOfAccount> GetHierarchyAsync(Guid id);
        Task<ChartOfAccount> GetAccountByIdAsync(Guid id);
        Task<List<ChartOfAccount>> GetAllAccountsAsync(Guid? id = null);
        Task<List<ChartOfAccount>> GetAllAccountsWithoutChildAsync(Guid id);
        Task<List<ChartOfAccount>> GetAllChildAccountsAsync(Guid id);
        Task AddAccountAsync(ChartOfAccount account);
        Task UpdateAccountAsync(ChartOfAccount account);
        Task DeleteAccountAsync(Guid id);
        Task<bool> IsParentAccount(Guid id);
    }

}
