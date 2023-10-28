using Microsoft.Extensions.DependencyInjection;

namespace SSU.DM.ExcelWriter.Internal;

internal static class WriterFactory
{
    internal static Writer Create(IServiceProvider provider, IReadOnlyList<Type> writerTypes)
        => new(writerTypes.Select(provider.GetRequiredService).ToList());
}