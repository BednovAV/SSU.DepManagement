using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

namespace SSU.DM.WebAssembly.Server.HostedServices;

public class WarmUpHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public WarmUpHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        using var scope = _scopeFactory.CreateScope();
        var appFormDao = scope.ServiceProvider.GetRequiredService<IApplicationFormDao>();
        var teachersDao = scope.ServiceProvider.GetRequiredService<ITeachersDao>();
        
        var appforms = appFormDao.GetAll();
        teachersDao.GetAll();
    }
}