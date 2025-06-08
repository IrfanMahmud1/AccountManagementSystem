using App.Qtech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Repositories
{
    public interface IVoucherRepository
    {
        Task SaveAsync(Voucher voucher);
    }
}
