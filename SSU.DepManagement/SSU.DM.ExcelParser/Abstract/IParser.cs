namespace SSU.DM.ExcelParser.Abstract;

public interface IParser
{
}

public interface IParser<out TResult> : IParser
{
    TResult? Parse(byte[] bytes);
}