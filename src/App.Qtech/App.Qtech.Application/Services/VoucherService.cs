using App.Qtech.Domain.Dtos;
using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public Task<List<VoucherDisplayDto>> GetAllVouchersAsync()
        {
            return _voucherRepository.GetAllDisplaysAsync();
        }

        public Task SaveVoucherAsync(Voucher voucher)
        {
            return _voucherRepository.SaveAsync(voucher);
        }
    }
}
