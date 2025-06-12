using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Entities;
using App.Qtech.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Services
{
    public interface IVoucherService
    {
        Task SaveVoucherAsync(Voucher voucher);
        Task<PagedResult<VoucherDisplayDto>> GetAllPaginatedVouchersAsync(int pageNumber, int pageSize);
    }
}
