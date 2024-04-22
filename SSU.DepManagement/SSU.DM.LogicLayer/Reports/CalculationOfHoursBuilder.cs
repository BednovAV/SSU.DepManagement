using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class CalculationOfHoursBuilder : ICalculationOfHoursBuilder, IEqualityComparer<Faculty>
{
    private readonly IExcelWriter _excelWriter;
    private readonly IApplicationFormDao _applicationFormDao;

    public CalculationOfHoursBuilder(
        IExcelWriter excelWriter,
        IApplicationFormDao applicationFormDao)
    {
        _excelWriter = excelWriter;
        _applicationFormDao = applicationFormDao;
    }

    public byte[] BuildReport(ISet<Guid> appFormIds)
    {
        var appForms = _applicationFormDao.GetAll(x => appFormIds.Contains(x.ApplicationFormId));
        var faculties = appForms
            .GroupBy(x => x.Faculty, x => x.Requests, this)
            .Select(FacultyDataBuilder.Build).ToList();
        
        var data = new CalculationOfHoursData { Faculties = faculties };
        return _excelWriter.Write(data).FileBytes;
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