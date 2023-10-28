namespace SSU.DM.Tools.Interface.Models;

public class WriteResult
{
    public bool IsSucceeded { get; set; }
    
    public WriteErrorType? ErrorType { get; set; }
    
    public byte[]? FileBytes { get; set; }
}

public enum WriteErrorType
{
    WriterNotFound,
    Undefined,
}