namespace SSU.DM.Tools.Interface.Models;

public class ParseResult<TResult>
{
    public bool IsSucceeded { get; set; }
    
    public ParseErrorType? ErrorType { get; set; }
    
    public TResult? Value { get; set; }
}

public enum ParseErrorType
{
    ParserNotFound,
    Undefined,
}