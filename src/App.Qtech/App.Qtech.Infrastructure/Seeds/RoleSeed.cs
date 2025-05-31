using App.Qtech.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Seeds
{
    public static class RoleSeed
    {
        public static ApplicationRole[] GetRoles()
        {
            return [
                new ApplicationRole
                {
                    Id = new Guid("2ABB9AF4-8E1B-49B6-B9E9-5266D0B9209C"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new DateTime(2025,5,31,1,2,3).ToString(),
                },
                 new ApplicationRole
                {
                    Id = new Guid("6A27DE7A-0004-430D-B1A1-7DB2BCCEADC1"),
                    Name = "Accountant",
                    NormalizedName = "ACCOUNTANT",
                    ConcurrencyStamp = new DateTime(2025,5,31,1,2,4).ToString(),
                },
                  new ApplicationRole
                {
                    Id = new Guid("EF7C185F-99D6-452C-8C14-F07D18485738"),
                    Name = "Viewer",
                    NormalizedName = "VIEWER",
                    ConcurrencyStamp = new DateTime(2025,5,31,1,2,5).ToString(),
                }
            ];
        }
    }
}
