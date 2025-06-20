﻿using App.Qtech.Domain.Entities;
using App.Qtech.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Services
{
    public interface IRoleModuleAccessService
    {
        Task<PagedResult<RoleModuleAccess>> GetPaginatedRoleModuleAccesses(int pageNumber, int pageSize);
        Task<bool> CanAcessAsync(string roleName, string moduleName, string operation);
        Task<List<string>> GetAllModuleNamesByRoleNameAsync(string role);
        Task<IList<RoleModuleAccess>> GetAllRoleModuleAccessesAsync();
        Task<RoleModuleAccess> GetRoleModuleAccessByIdAsync(Guid id);
        Task<bool> CreateRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess);
        Task<bool> EditRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess);
        Task<bool> RemoveRoleModuleAccessAsync(Guid id);
    }
}
