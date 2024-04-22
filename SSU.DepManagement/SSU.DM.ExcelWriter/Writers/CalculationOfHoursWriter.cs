using Models.Request;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.ExcelUtils;
using SSU.DM.ExcelWriter.Abstract;
using SSU.DM.ExcelWriter.Internal;

namespace SSU.DM.ExcelWriter.Writers;

[DataWriter]
public class CalculationOfHoursWriter : IWriter<CalculationOfHoursData>
{
    private const string TEMPLATE_KEY = "CALCULATION_OF_HOURS_TEMPLATE";
    
    private readonly IFilesStorageDao _filesStorageDao;

    public CalculationOfHoursWriter(IFilesStorageDao filesStorageDao)
    {
        _filesStorageDao = filesStorageDao;
    }

    public byte[] GetExcel(CalculationOfHoursData data)
    {
        using var excelDoc = _filesStorageDao.ReadExcel(TEMPLATE_KEY);
        var firstWorksheet = excelDoc.GetFirstWorksheetEditor();
        var currentRow = 11;
        FacultyFiller.FillFaculties(firstWorksheet, data.Faculties, ref currentRow);

        return excelDoc.GetAsByteArray();
    }
}