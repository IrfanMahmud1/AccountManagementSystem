using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace App.Qtech.Web.Areas.Admin.Models
{
    public class CreateVoucherModel
    {
        public DateTime Date { get; set; }

        [Required]
        public string ReferenceNo { get; set; }

        [Required]
        public VoucherType Type { get; set; }

        public List<VoucherEntry> Entries { get; set; } = new();
    }

}
