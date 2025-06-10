using App.Qtech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Services
{
    public interface IRoleModuleAccessService
    {
        Task<List<string>> GetAllModuleNamesByRoleNameAsync(string role);
        Task<IList<RoleModuleAccess>> GetAllRoleModuleAccessesAsync();

        Task<bool> CreateRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess);
        Task<bool> EditRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess);
        Task<bool> RemoveRoleModuleAccessAsync(Guid id);
    }
}
