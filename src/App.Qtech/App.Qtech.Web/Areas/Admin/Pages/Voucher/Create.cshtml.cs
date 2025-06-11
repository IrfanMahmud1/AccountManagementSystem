using App.Qtech.Application.Services;
using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Services;
using App.Qtech.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.Voucher
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<CreateModel> _logger;
        private readonly IChartOfAccountService _chartOfAccountService;

        [BindProperty]
        public CreateVoucherModel VoucherVM { get; set; }
        public List<App.Qtech.Domain.Entities.ChartOfAccount> Accounts { get; set; } = new();
        public CreateModel(IVoucherService voucherService, 
            IRoleModuleAccessService roleModuleAccessService, 
            ILogger<CreateModel> logger,
            IChartOfAccountService chartOfAccountService)
        {
            _voucherService = voucherService;
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "Voucher", "Create"))
                {
                    _logger.LogWarning("User does not have access to Create Voucher.");
                    return RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            VoucherVM = new CreateVoucherModel
            {
                Date = DateTime.Today,
                Entries = new List<VoucherEntry>
            {
                new VoucherEntry() // one row by default
            }
            };
            try
            {
                Accounts = await _chartOfAccountService.GetAllAccountsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load chart of accounts.");
                ModelState.AddModelError(string.Empty, "Failed to load accounts. Please try again later.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var voucher = new App.Qtech.Domain.Entities.Voucher
                {
                    Id = Guid.NewGuid(),
                    Date = VoucherVM.Date,
                    ReferenceNo = VoucherVM.ReferenceNo,
                    Type = VoucherVM.Type.ToString(),
                    Entries = VoucherVM.Entries.Select(e => new VoucherEntry
                    {
                        Id = Guid.NewGuid(),
                        VoucherId = Guid.Empty, // assigned in repo
                        AccountName = e.AccountName,
                        Debit = e.Debit,
                        Credit = e.Credit
                    }).ToList()
                };

                await _voucherService.SaveVoucherAsync(voucher);

                TempData["Success"] = "Voucher saved successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }

}
