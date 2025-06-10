using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class IndexModel : PageModel
    {
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public IndexModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<App.Qtech.Domain.Entities.RoleModuleAccess> RoleModuleAccess { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RoleModuleAccess = await _context.RoleModuleAccesses.ToListAsync();
        }
    }
}
