namespace SSU.DM.WebAssembly.Shared.Models;

public class CreateTeacherLinkResponse
{
    public ResponseType Type { get; set; }

    public string Message { get; set; }

    public enum ResponseType
    {
        Success,
        Error,
        Warning
    }
}