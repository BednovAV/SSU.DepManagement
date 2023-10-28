namespace SSU.DM.ExcelParser.Exceptions;

public class MapToResultException : Exception
{
    private const string ERROR_MSG = "Error while map data to result";

    public MapToResultException()
        : base(ERROR_MSG) { }

    public MapToResultException(Exception exception)
        : base(ERROR_MSG, exception) { }
}
