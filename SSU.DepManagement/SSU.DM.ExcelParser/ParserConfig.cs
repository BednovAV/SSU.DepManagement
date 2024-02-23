using Microsoft.Extensions.DependencyInjection;
using Models.Request;
using SSU.DM.ExcelParser.Abstract;
using SSU.DM.ExcelParser.Parsers;

namespace SSU.DM.ExcelParser;

public static class ParserConfig
{
    public static IServiceCollection RegisterParsers(this IServiceCollection services)
        => services.AddSingleton<IParser, RequestParser>();
    
    internal static IParser? GetParser<TParserResult>(TParserResult resultObj)
        => resultObj switch
        {
            List<ParsedRequest> => new RequestParser(),
            _ => null
        };
}