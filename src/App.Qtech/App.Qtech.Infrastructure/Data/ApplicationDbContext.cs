using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Identity;
using App.Qtech.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace App.Qtech.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {

        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        private const string SqlConlumnType = "decimal(18,2)";
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherEntry> VoucherEntries { get; set; }
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>().HasData(RoleSeed.GetRoles());
            builder.Entity<VoucherEntry>(entity =>
            {
                entity.Property(e => e.Debit).
                    HasColumnType(SqlConlumnType);
                entity.Property(e => e.Debit).
                    HasColumnType(SqlConlumnType);
            });

            base.OnModelCreating(builder);
        }
    }
}
