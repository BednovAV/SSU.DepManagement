using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Request;

public interface IRequestAccessor
{
    (string name, byte[] bytes) GetFile(string? fileKey = null);
    
    IReadOnlyList<ApplicationFormViewItem> GetApplicationForms();
}
