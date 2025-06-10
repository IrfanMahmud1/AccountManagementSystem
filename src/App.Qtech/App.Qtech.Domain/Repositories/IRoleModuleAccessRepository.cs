using App.Qtech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Repositories
{
    public interface IRoleModuleAccessRepository
    {
        Task<IEnumerable<RoleModuleAccess>> GetAllAsync();
        Task<List<string>> GetAsync(string role);

        Task<bool> AddAsync(RoleModuleAccess moduleAccess);

        Task<bool> UpdateAsync(RoleModuleAccess moduleAccess);

        Task<bool> DeleteAsync(Guid id);
    }
}
