using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class CalculationOfHoursDataDataBuilder : ICalculationOfHoursDataBuilder, IEqualityComparer<Faculty>
{
    private readonly IApplicationFormDao _applicationFormDao;

    public CalculationOfHoursDataDataBuilder(
        IApplicationFormDao applicationFormDao)
    {
        _applicationFormDao = applicationFormDao;
    }

    public CalculationOfHoursData BuildData(ISet<Guid> appFormIds)
    {
        var appForms = _applicationFormDao.GetAll(x => appFormIds.Contains(x.ApplicationFormId));
        var studyForms = StudyFormDataBuilder.Build(appForms.SelectMany(x => x.Requests)).ToList();
        
        var data = new CalculationOfHoursData
        {
            StudyForms = studyForms,
            StudyYear = "2024/2025"
        };
        return data;
    }

    

    public bool Equals(Faculty? x, Faculty? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Faculty obj) => obj.Id;
}