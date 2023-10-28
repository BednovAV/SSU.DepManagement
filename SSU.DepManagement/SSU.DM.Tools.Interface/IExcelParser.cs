using SSU.DM.Tools.Interface.Models;

namespace SSU.DM.Tools.Interface;

public interface IExcelParser
{
    ParseResult<TResult> Parse<TResult>(byte[] bytes)
        where TResult: new();
}