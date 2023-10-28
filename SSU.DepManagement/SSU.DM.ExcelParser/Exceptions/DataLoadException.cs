namespace SSU.DM.ExcelParser.Exceptions;

public class DataLoadException : Exception
{
    private const string ERROR_MSG = "Error while load from excel";

    public DataLoadException()
        : base(ERROR_MSG) { }

    public DataLoadException(Exception exception)
        : base(ERROR_MSG, exception) { }
}
