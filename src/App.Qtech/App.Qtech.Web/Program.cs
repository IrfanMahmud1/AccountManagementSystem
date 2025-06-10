using App.Qtech.Web;
using App.Qtech.Web.Data;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using App.Qtech.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using App.Qtech.Infrastructure.Seeds;


var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateBootstrapLogger();
try
{
    Log.Information("Application Starting...... ");

    var builder = WebApplication.CreateBuilder(args);
    var migrationAssembly = (typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name ?? throw new InvalidOperationException("Migration assembly name not found."));
    // Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    #region Identity Configuration
    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    #endregion
    #region Autofac Configuration
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly));
    });
    #endregion
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddRazorPages();
    builder.Services.AddAuthorization();
    var app = builder.Build();

    #region UserSeed configuration
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await UserSeed.SeedUserAsync(services);
    }
    #endregion

    #region RoleModuleAccessSeed configuration
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await RoleModuleAccessSeed.SeedRoleModuleAccessSeedAsync(services);
    }
    #endregion

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();

    Log.Information("Application Started...... ");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Crashed");
}
finally
{
    await Log.CloseAndFlushAsync();
}



