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
        Task<List<ChartOfAccount>> GetAllAsync();
        Task AddAsync(ChartOfAccount account);
        Task UpdateAsync(ChartOfAccount account);
        Task DeleteAsync(int id);
    }
}
