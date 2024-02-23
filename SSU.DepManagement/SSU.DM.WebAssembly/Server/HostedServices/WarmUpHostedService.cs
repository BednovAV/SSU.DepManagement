using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

namespace SSU.DM.WebAssembly.Server.HostedServices;

public class WarmUpHostedService : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;

    public WarmUpHostedService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        using var scope = scopeFactory.CreateScope();
        var appFormDao = scope.ServiceProvider.GetRequiredService<IApplicationFormDao>();
        
        appFormDao.GetAll();
    }
}