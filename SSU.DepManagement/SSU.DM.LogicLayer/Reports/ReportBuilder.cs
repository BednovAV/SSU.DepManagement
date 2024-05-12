using Models.Request;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.Tools.Interface;

namespace SSU.DM.LogicLayer.Reports;

public class ReportBuilder : IReportBuilder
{
    private readonly IExcelWriter _excelWriter;
    private readonly ICalculationOfHoursDataBuilder _calculationOfHoursDataBuilder;
    private readonly IStudyAssignmentReportDataBuilder _studyAssignmentReportDataBuilder;
    private readonly IDistributionReportDataBuilder _distributionReportDataBuilder;

    public ReportBuilder(
        IExcelWriter excelWriter,
        ICalculationOfHoursDataBuilder calculationOfHoursDataBuilder,
        IStudyAssignmentReportDataBuilder studyAssignmentReportDataBuilder,
        IDistributionReportDataBuilder distributionReportDataBuilder)
    {
        _calculationOfHoursDataBuilder = calculationOfHoursDataBuilder;
        _distributionReportDataBuilder = distributionReportDataBuilder;
        _studyAssignmentReportDataBuilder = studyAssignmentReportDataBuilder;
        _excelWriter = excelWriter;
    }

    public byte[] BuildReport(GenerateReportsRequest request)
    {
        var data = new List<object>();
        if (request.CalculationOfHours)
        {
            data.Add(_calculationOfHoursDataBuilder.BuildData(request.AppFormIds));
        }
        
        if (request.DistributionReport)
        {
            data.Add(_distributionReportDataBuilder.BuildData(request.AppFormIds));
        }
        
        if (request.StudyAssignmentCard)
        {
            data.Add(_studyAssignmentReportDataBuilder.BuildData(request.AppFormIds));
        }
        
        return _excelWriter.Write(data.ToArray()).FileBytes;
    }
}