using SSU.DM.Tools.Interface.Models;

namespace SSU.DM.Tools.Interface;

public interface IExcelWriter
{
    WriteResult Write<T>(T data)
        where T: new();
}