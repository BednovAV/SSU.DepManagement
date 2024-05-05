using Microsoft.AspNetCore.Identity;
using SSU.DM.Authorization.Db;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.HostedServices;

public class InitializeDataHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public InitializeDataHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        // Warn up
        using var scope = _scopeFactory.CreateScope();
        var appFormDao = scope.ServiceProvider.GetRequiredService<IApplicationFormDao>();
        var teachersDao = scope.ServiceProvider.GetRequiredService<ITeachersDao>();
        
        appFormDao.GetAll();
        teachersDao.GetAll();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await AuthDataInitializer.InitialiseAsync(roleManager, Roles.AllRoles);
    }
}