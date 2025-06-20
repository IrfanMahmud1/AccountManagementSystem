﻿using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using App.Qtech.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Application.Services
{
    public class ChartOfAccountService : IChartOfAccountService
    {
        private readonly IChartOfAccountRepository _repository;
        private ILogger<ChartOfAccountService> _logger;
        public ChartOfAccountService(IChartOfAccountRepository repository,
            ILogger<ChartOfAccountService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ChartOfAccount> GetAccountByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
        public async Task<List<ChartOfAccount>> GetAllAccountsAsync(Guid? id = null) => await _repository.GetAllAsync(id);
        public async Task<List<ChartOfAccount>> GetAllAccountsWithoutChildAsync(Guid id) => await _repository.GetAllWithNoChildAsync(id);
        public async Task<List<ChartOfAccount>> GetAllChildAccountsAsync(Guid id) => await _repository.GetAllChildsAsync(id);
        public async Task AddAccountAsync(ChartOfAccount account) => await _repository.CreateAsync(account);
        public async Task UpdateAccountAsync(ChartOfAccount account) => await _repository.UpdateAsync(account);
        public async Task DeleteAccountAsync(Guid id) => await _repository.DeleteAsync(id);
        public async Task<bool> IsParentAccount(Guid id) =>
            await _repository.GetChildCountByParentIdAsync(id) > 0;
        public async Task<ChartOfAccount> GetHierarchyAsync(Guid id)
        {
            var root = await GetAccountByIdAsync(id);
            if (root == null) return null;

            return await BuildTree(root);
        }

        private async Task<ChartOfAccount> BuildTree(ChartOfAccount node)
        {
            var viewModel = new ChartOfAccount
            {
                Id = node.Id,
                Name = node.Name,
                ParentId = node.ParentId,
                AccountType = node.AccountType,
                IsActive = node.IsActive
            };

            var children = await _repository.GetAllChildsAsync(node.Id);
            foreach (var child in children)
            {
                viewModel.Children.Add(await BuildTree(child));
            }

            return viewModel;
        }

        public async Task<bool> IsDuplicate(string name)
        {
            return await _repository.IsExistAsync(name);
        }

        public async Task<PagedResult<ChartOfAccount>> GetPaginatedChartOfAccounts(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            try
            {
                var accounts = await _repository.GetAllAsync();
                var totalItems = accounts.Count;

                accounts = accounts.OrderBy(a => a.Name)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                return new PagedResult<ChartOfAccount>
                {
                    Items = accounts.ToList(),
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to retrieve accounts");
                throw new InvalidOperationException("An error occurred while fetching paginated accounts.", ex);
            }
            
        }
    }

}
