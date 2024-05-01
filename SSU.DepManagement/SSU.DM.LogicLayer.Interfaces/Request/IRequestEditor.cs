using Models.View;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.LogicLayer.Interfaces.Request;

public interface IRequestEditor
{
    Task<Guid> UploadFromStream(string fileName, Stream stream);
    
    Task<bool> DeleteAsync(Guid appFormId);
    
    void CreateFacultyLinkAsync(Guid appFormId, int facultyId);
    
    CreateTeacherLinkResponse CreateTeacherLink(int requestId, long? teacherId);
    
    void AssignTeachers(HashSet<Guid> appFromIds);
}
