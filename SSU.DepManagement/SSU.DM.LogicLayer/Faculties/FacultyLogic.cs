using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.LogicLayer.Interfaces.Faculty;

namespace SSU.DM.LogicLayer.Faculties;

public class FacultyLogic : IFacultyLogic
{
    private readonly IFacultyDao _facultyDao;

    public FacultyLogic(IFacultyDao facultyDao)
    {
        _facultyDao = facultyDao;
    }

    public IEnumerable<FacultyViewItem> GetAll() =>
        _facultyDao.GetAll()
            .Select(x => new FacultyViewItem
            {
                Id = x.Id,
                Name = x.Name,
                NameDat = x.NameDat
            })
            .ToList();

    public void AddFaculty(FacultyViewItem faculty)
    {
        _facultyDao.Add(new DataAccessLayer.DbEntities.Faculty
        {
            Name = faculty.Name,
            NameDat = faculty.NameDat
        });
    }

    public void DeleteFaculty(int id)
    {
        _facultyDao.DeleteById(id);
    }

    public void UpdateFaculty(FacultyViewItem faculty)
    {
        _facultyDao.Update(new DataAccessLayer.DbEntities.Faculty
        {
            Id = faculty.Id,
            Name = faculty.Name,
            NameDat = faculty.NameDat
        });
    }
}