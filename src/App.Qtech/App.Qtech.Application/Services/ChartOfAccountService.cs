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

        public async Task AddAccountAsync(ChartOfAccount account) => await _repository.AddAsync(account);

    }

}
