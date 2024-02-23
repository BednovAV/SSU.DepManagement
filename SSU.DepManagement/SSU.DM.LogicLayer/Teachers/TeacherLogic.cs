using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Teachers;

namespace SSU.DM.LogicLayer.Teachers;

public class TeacherLogic : ITeacherLogic
{
    private readonly ITeachersDao _teachersDao;
    private readonly ISemesterDao _semesterDao;
    private readonly ITeacherCapacityDao _teacherCapacityDao;


    public TeacherLogic(
        ITeachersDao teachersDao,
        ISemesterDao semesterDao,
        ITeacherCapacityDao teacherCapacityDao)
    {
        _teachersDao = teachersDao;
        _semesterDao = semesterDao;
        _teacherCapacityDao = teacherCapacityDao;
    }

    public IReadOnlyList<TeacherViewItem> GetAll()
    {
        return _teachersDao.GetAll().Select(x => x.ToViewItem()).ToList();
    }

    public IReadOnlyList<TeacherCapacitiesViewItem> GetTeacherCapacities()
    {
        var semesters = _semesterDao.GetAll();
        return _teachersDao.GetAll()
            .Select(teacher => CreateTeacherCapacitiesViewItem(teacher, semesters))
            .ToList();
    }

    public void UpdateCapacities(long teacherId, Dictionary<long, int> totalHoursBySemester)
    {
        var existedSemesters = _teacherCapacityDao.GetTeacherSemesters(teacherId);

        var itemsToInsert = new List<TeacherCapacity>();
        var itemsToUpdate = new List<TeacherCapacity>();

        foreach (var item in totalHoursBySemester)
        {
            var entity = new TeacherCapacity
            {
                TeacherId = teacherId,
                SemesterId = item.Key,
                Hours = item.Value
            };
            
            if (existedSemesters.Contains(item.Key))
            {
                itemsToUpdate.Add(entity);
            }
            else
            {
                itemsToInsert.Add(entity);
            }
        }

        _teacherCapacityDao.Update(itemsToUpdate);
        _teacherCapacityDao.Add(itemsToInsert);
    }

    private TeacherCapacitiesViewItem CreateTeacherCapacitiesViewItem(
        Teacher teacher,
        IReadOnlyList<Semester> semesters)
    {

        return new TeacherCapacitiesViewItem
        {
            Teacher = teacher.ToViewItem(),
            CapacityBySemester = semesters.ToDictionary(
                semester => semester.Id,
                semester =>
                {
                    var teacherCapacity = teacher.Capacities
                        .FirstOrDefault(capacity => capacity.SemesterId == semester.Id);
                    return new CapacityView(
                        TotalHours: teacherCapacity?.Hours ?? 0,
                        AllocatedHours: teacher.Requests
                            //.Where(request => request.SemesterId == teacherCapacity.SemesterId)
                            .Sum(request => request.TotalHours));
                })
        };
    }

    public void Create(TeacherViewItem viewItem)
    {
        _teachersDao.Add(new Teacher
        {
            FirstName = viewItem.FirstName,
            MiddleName = viewItem.MiddleName,
            LastName = viewItem.LastName,
            JobTitleId = viewItem.JobTitle.Id,
            Rate = viewItem.Rate,
        });
    }

    public void Update(TeacherViewItem viewItem)
    {
        _teachersDao.Update(new Teacher
        {
            Id = viewItem.Id,
            FirstName = viewItem.FirstName,
            MiddleName = viewItem.MiddleName,
            LastName = viewItem.LastName,
            JobTitleId = viewItem.JobTitle?.Id,
            Rate = viewItem.Rate,
        });
    }

    public void Delete(long id)
    {
        _teachersDao.DeleteById(id);
    }
}