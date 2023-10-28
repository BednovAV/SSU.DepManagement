using Microsoft.Extensions.DependencyInjection;

namespace SSU.DM.ServiceFactory;

public static class ServiceFactory
{
    public static IServiceProvider Provider { private get; set; }

    public static T Resolve<T>() => Provider.GetService<T>();
}