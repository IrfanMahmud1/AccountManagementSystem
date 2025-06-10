using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Domain.Entities
{
    public class RoleModuleAccess
    {
        [Key]
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public bool HasAccess { get; set; }
        public string Operation { get; set; } // Create, Update, Delete, View
    }
}
