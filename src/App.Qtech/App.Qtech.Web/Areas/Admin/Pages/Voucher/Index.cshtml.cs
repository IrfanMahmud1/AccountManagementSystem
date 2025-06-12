using App.Qtech.Application.Services;
using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Services;
using App.Qtech.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.Voucher
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        private readonly ILogger<IndexModel> _logger;
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        public IndexModel(IVoucherService voucherService, ILogger<IndexModel> logger, IRoleModuleAccessService roleModuleAccessService)
        {
            _voucherService = voucherService;
            _logger = logger;
            _roleModuleAccessService = roleModuleAccessService;
        }
        public PagedResult<VoucherDisplayDto> VoucherRows { get; set; } = new();
        public async Task<IActionResult> OnGet(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "Voucher", "View"))
                {
                    _logger.LogWarning("User does not have access to View Voucher.");
                    return RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            try
            {
                VoucherRows = await _voucherService.GetAllPaginatedVouchersAsync(pageNumber,pageSize);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                _logger.LogError(ex, "An error occurred while loading vouchers.");
                ModelState.AddModelError(string.Empty, "An error occurred while loading vouchers.");
            }
            return Page();
        }
    }
}
