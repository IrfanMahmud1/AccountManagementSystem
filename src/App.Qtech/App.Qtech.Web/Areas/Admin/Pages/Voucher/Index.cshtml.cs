using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Qtech.Web.Areas.Admin.Pages.Voucher
{
    public class IndexModel : PageModel
    {
        private readonly IVoucherService _voucherService;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IVoucherService voucherService, ILogger<IndexModel> logger)
        {
            _voucherService = voucherService;
            _logger = logger;
        }
        public List<VoucherDisplayDto> VoucherRows { get; set; } = new();
        public async Task OnGet()
        {
            try
            {
                VoucherRows = await _voucherService.GetAllVouchersAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                _logger.LogError(ex, "An error occurred while loading vouchers.");
                ModelState.AddModelError(string.Empty, "An error occurred while loading vouchers.");
            }
        }
    }
}
