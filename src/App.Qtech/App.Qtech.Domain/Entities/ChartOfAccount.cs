﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Entities
{
    public class ChartOfAccount : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string AccountType { get; set; } // Asset, Liability, Income, etc.
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public List<ChartOfAccount> Children { get; set; } = new();
    }

}
