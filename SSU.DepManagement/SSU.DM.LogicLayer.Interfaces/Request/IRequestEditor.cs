namespace SSU.DM.LogicLayer.Interfaces.Request;

public interface IRequestEditor
{
    Task<Guid> UploadFromStream(string fileName, Stream stream);
    
    Task<bool> DeleteAsync(Guid appFormId);
    
    void CreateFacultyLinkAsync(Guid appFormId, int facultyId);
    
    string? CreateTeacherLink(int requestId, long? teacherId);
    
    void AssignTeachers(HashSet<Guid> appFromIds);
}
