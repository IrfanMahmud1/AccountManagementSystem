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
    public class ChartOfAccountService : IChartOfAccountService
    {
        private readonly IChartOfAccountRepository _repository;

        public ChartOfAccountService(IChartOfAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<ChartOfAccount> GetAccountByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
        public async Task<List<ChartOfAccount>> GetAllAccountsAsync() => await _repository.GetAllAsync();
        public async Task AddAccountAsync(ChartOfAccount account) => await _repository.CreateAsync(account);
        public async Task UpdateAccountAsync(ChartOfAccount account) => await _repository.UpdateAsync(account);
        public async Task DeleteAccountAsync(Guid id) => await _repository.DeleteAsync(id);
        public async Task<bool> IsParentAccount(Guid id) =>
            await _repository.GetChildCountByParentIdAsync(id) > 0;

    }

}
