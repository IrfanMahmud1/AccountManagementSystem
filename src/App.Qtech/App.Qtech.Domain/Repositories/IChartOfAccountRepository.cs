using App.Qtech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Repositories
{
    public interface IChartOfAccountRepository
    {
        Task<ChartOfAccount> GetByIdAsync(Guid id);
        Task<List<ChartOfAccount>> GetAllAsync(Guid? id = null);
        Task<List<ChartOfAccount>> GetAllWithNoChildAsync(Guid id);
        Task<List<ChartOfAccount>> GetAllChildsAsync(Guid id);
        Task<bool> CreateAsync(ChartOfAccount account);
        Task<bool> UpdateAsync(ChartOfAccount account);
        Task<bool> DeleteAsync(Guid id);
        Task<int> GetChildCountByParentIdAsync(Guid id);
    }
}
