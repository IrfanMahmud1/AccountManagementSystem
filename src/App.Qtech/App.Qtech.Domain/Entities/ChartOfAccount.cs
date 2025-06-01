using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Entities
{
    public class ChartOfAccount : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string AccountType { get; set; } // Asset, Liability, Income, etc.
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
