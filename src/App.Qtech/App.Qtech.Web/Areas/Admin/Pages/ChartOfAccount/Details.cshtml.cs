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
using App.Qtech.Domain.Repositories;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IChartOfAccountService _chartOfAccountService;

        public DetailsModel(ILogger<DetailsModel> logger, IChartOfAccountService chartOfAccountService)
        {
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (Guid.Empty == id )
            {
                return NotFound();
            }

            try
            {
                ChartOfAccount = await _chartOfAccountService.GetHierarchyAsync(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve child account details");
                return RedirectToPage("./Error");
            }

            return Page();
        }
    }
}
