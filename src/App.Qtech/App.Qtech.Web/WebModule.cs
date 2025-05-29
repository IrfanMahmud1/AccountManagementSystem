using App.Qtech.Infrastructure.Data;
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
            base.Load(builder);
        }
    }
}
