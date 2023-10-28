namespace SSU.DM.LogicLayer.Interfaces.Request;

public interface IRequestEditor
{
    Task<Guid> UploadFromStream(string fileName, Stream stream);
    
    Task<bool> DeleteAsync(Guid appFormId);
}
