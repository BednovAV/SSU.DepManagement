using Models.Request;
using SSU.DM.Tools.Interface;
using SSU.DM.Tools.Interface.Models;
using SSU.DM.ExcelParser.Abstract;

namespace SSU.DM.ExcelParser;

public class Parser : IExcelParser
{
    private readonly IEnumerable<IParser> _parsers;

    public Parser(IEnumerable<IParser> parsers)
    {
        _parsers = parsers;
    }

    public ParseResult<TResult> Parse<TResult>(byte[] bytes)
        where TResult: new()
    {
        var parser = GetParser<TResult>();
        if (parser == null)
            return new()
            {
                IsSucceeded = false,
                ErrorType = ParseErrorType.ParserNotFound,
            };
        
        var parsedValue = parser.Parse(bytes);
        var isParsed = parsedValue != null;
        return new()
        {
            IsSucceeded = isParsed,
            ErrorType = !isParsed ? ParseErrorType.Undefined : null,
            Value = parsedValue,
        };
    }
    
    private static IParser<TResultType>? GetParser<TResultType>()
        where TResultType : new()
    {
        var parser = ParserConfig.GetParser(new TResultType());
        return parser as IParser<TResultType>;
    }
}
