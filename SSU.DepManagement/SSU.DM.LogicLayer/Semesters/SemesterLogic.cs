using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.LogicLayer.Interfaces.Semesters;

namespace SSU.DM.LogicLayer.Semesters;

public class SemesterLogic : ISemesterLogic
{
    private readonly ISemesterDao _semesterDao;

    public SemesterLogic(ISemesterDao semesterDao)
    {
        _semesterDao = semesterDao;
    }

    public IReadOnlyList<SemesterViewItem> GetAll()
    {
        return _semesterDao.GetAll()
            .Select(x => x.ToViewItem())
            .ToList();
    }
}