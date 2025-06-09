using App.Qtech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Entities
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public List<VoucherEntry>? Entries { get; set; }
    }

}
