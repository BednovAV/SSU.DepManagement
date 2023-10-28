using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Faculty;

public interface IFacultyLogic
{
    IEnumerable<FacultyViewItem> GetAll();
    
    void AddFaculty(FacultyViewItem faculty);
    
    void DeleteFaculty(int id);
    
    void UpdateFaculty(FacultyViewItem faculty);
}