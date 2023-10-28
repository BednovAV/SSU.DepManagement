using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Models.Request;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;
using SSU.DM.ExcelWriter.Writers;
using SSU.DM.Tools.Interface;

namespace SSU.DM.ExcelWriter;

public static class WriterConfig
{
    public static IServiceCollection RegisterWriters(this IServiceCollection services)
    {
        var writerTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.CustomAttributes
                .Any(c => c.AttributeType == typeof(DataWriterAttribute)))
            .ToList();
        writerTypes.ForEach(x => services.AddScoped(x));
        return services.AddScoped<IExcelWriter, Writer>(provider => WriterFactory.Create(provider, writerTypes));
    }
}