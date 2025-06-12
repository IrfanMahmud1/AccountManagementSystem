using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using App.Qtech.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly ILogger<VoucherService> _logger;   
        public VoucherService(IVoucherRepository voucherRepository,ILogger<VoucherService> logger)
        {
            _voucherRepository = voucherRepository;
            _logger = logger;
        }

        public async Task<PagedResult<VoucherDisplayDto>> GetAllPaginatedVouchersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            try
            {
                var vouchers = await _voucherRepository.GetAllDisplaysAsync();
                var totalItems = vouchers.Count;

                vouchers = vouchers.OrderBy(a => a.AccountName)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

                return new PagedResult<VoucherDisplayDto>
                {
                    Items = vouchers.ToList(),
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve vouchers");
                throw new InvalidOperationException("An error occurred while fetching paginated vouchers.", ex);
            }
        }

        public Task SaveVoucherAsync(Voucher voucher)
        {
            return _voucherRepository.SaveAsync(voucher);
        }
    }
}
