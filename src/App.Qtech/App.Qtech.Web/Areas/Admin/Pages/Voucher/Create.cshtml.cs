using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Services;
using App.Qtech.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Qtech.Web.Areas.Admin.Pages.Voucher
{
    public class CreateModel : PageModel
    {
        private readonly IVoucherService _voucherService;

        [BindProperty]
        public CreateVoucherModel VoucherVM { get; set; }

        public CreateModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public void OnGet()
        {
            VoucherVM = new CreateVoucherModel
            {
                Date = DateTime.Today,
                Entries = new List<VoucherEntry>
            {
                new VoucherEntry() // one row by default
            }
            };
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
