using App.Qtech.Application.Services;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Infrastructure.Repositories;
using Autofac;
using System.ComponentModel;

namespace App.Qtech.Web
{
    public class WebModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
             .WithParameter("connectionString", _connectionString)
             .WithParameter("migrationAssembly", _migrationAssembly)
             .InstancePerLifetimeScope();
            builder.RegisterType<ChartOfAccountRepository>().As<IChartOfAccountRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ChartOfAccountService>().As<IChartOfAccountService>()
               .InstancePerLifetimeScope(); 
            builder.RegisterType<VoucherRepository>().As<IVoucherRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<VoucherService>().As<IVoucherService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<RoleModuleAccessRepository>().As<IRoleModuleAccessRepository>()
             .InstancePerLifetimeScope();
            builder.RegisterType<RoleModuleAccessService>().As<IRoleModuleAccessService>()
               .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
