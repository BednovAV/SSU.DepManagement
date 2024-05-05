using Models.ConfigSections;
using Models.Extensions;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ServiceFactory;
using SSU.DM.WebAssembly.Server;
using SSU.DM.WebAssembly.Server.HostedServices;

namespace SSU.DepManagement.WebAssembly;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        var config = builder.Configuration;
        var dataConfiguration = config.GetSection<DataConfigurationConfigSection>();
        var connectionString = config.GetConnectionString(dataConfiguration.SelectedConnection);
        builder.Services.RegisterApplicationDependencies(connectionString);
        builder.Services.AddHostedService<InitializeDataHostedService>();
        ServiceFactory.Provider = builder.Services.BuildServiceProvider();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}