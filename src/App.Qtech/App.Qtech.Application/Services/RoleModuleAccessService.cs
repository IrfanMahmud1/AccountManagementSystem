using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using App.Qtech.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RoleModuleAccessService> _logger;
        public RoleModuleAccessService(IRoleModuleAccessRepository roleModuleAccessRepository,
            ILogger<RoleModuleAccessService> logger)
        {
            _roleModuleAccessRepository = roleModuleAccessRepository;
            _logger = logger;
        }

        public async Task<PagedResult<RoleModuleAccess>> GetPaginatedRoleModuleAccesses(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            try
            {
                var roleModuleAccesses = await _roleModuleAccessRepository.GetAllAsync();
                var totalItems = roleModuleAccesses.Count;

                roleModuleAccesses = roleModuleAccesses.OrderBy(a => a.Operation)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                return new PagedResult<RoleModuleAccess>
                {
                    Items = roleModuleAccesses.ToList(),
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve RoleModuleAccesses");
                throw new InvalidOperationException("An error occurred while fetching paginated RoleModuleAccesses.", ex);
            }

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
