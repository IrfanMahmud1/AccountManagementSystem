using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Application.Services
{
    public class RoleModuleAccessService : IRoleModuleAccessService
    {
        private readonly IRoleModuleAccessRepository _roleModuleAccessRepository;
        public RoleModuleAccessService(IRoleModuleAccessRepository roleModuleAccessRepository)
        {
            _roleModuleAccessRepository = roleModuleAccessRepository;
        }

        public async Task<bool> CanAcessAsync(string roleName, string moduleName, string operation)
        {
            return await _roleModuleAccessRepository.HasAcess(roleName, moduleName,operation);
        }

        public async Task<bool> CreateRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess)
        {
            return await _roleModuleAccessRepository.AddAsync(roleModuleAccess);
        }

        public async Task<bool> EditRoleModuleAccessAsync(RoleModuleAccess roleModuleAccess)
        {
            return await _roleModuleAccessRepository.UpdateAsync(roleModuleAccess);
        }

        public async Task<List<string>> GetAllModuleNamesByRoleNameAsync(string role)
        {
            return await _roleModuleAccessRepository.GetAsync(role);
        }

        public async Task<IList<RoleModuleAccess>> GetAllRoleModuleAccessesAsync()
        {
            return await _roleModuleAccessRepository.GetAllAsync();
        }

        public async Task<RoleModuleAccess> GetRoleModuleAccessByIdAsync(Guid id)
        {
            return await _roleModuleAccessRepository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveRoleModuleAccessAsync(Guid id)
        {
            return await _roleModuleAccessRepository.DeleteAsync(id);
        }
    }
}
