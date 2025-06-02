using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Domain.Services;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    public class IndexModel : PageModel
    {
        private readonly IChartOfAccountService _chartOfAccountService;

        public IndexModel(IChartOfAccountService chartOfAccountService)
        {
            _chartOfAccountService = chartOfAccountService;
        }

        public IList<App.Qtech.Domain.Entities.ChartOfAccount> ChartOfAccount { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ChartOfAccount = await _chartOfAccountService.GetAllAccountsAsync();
        }
    }
}
