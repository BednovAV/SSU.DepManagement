using Microsoft.AspNetCore.Mvc;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.WebAssembly.Shared;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class ReportsController : ControllerBase
{
    private readonly ICalculationOfHoursBuilder _calculationOfHoursBuilder;
    private readonly IDistributionReportBuilder _distributionReportBuilder;
    private readonly IStudyAssignmentReportBuilder _studyAssignmentReportBuilder;

    public ReportsController(
        ICalculationOfHoursBuilder calculationOfHoursBuilder,
        IDistributionReportBuilder distributionReportBuilder,
        IStudyAssignmentReportBuilder studyAssignmentReportBuilder)
    {
        _calculationOfHoursBuilder = calculationOfHoursBuilder;
        _distributionReportBuilder = distributionReportBuilder;
        _studyAssignmentReportBuilder = studyAssignmentReportBuilder;
    }

    [HttpGet(RouteConstants.REPORTS_CALCULATION_OF_HOURS)]
    public ActionResult GetCalculationOfHoursReport(HashSet<Guid> appFormIds)
    {
        return File(_calculationOfHoursBuilder.BuildReport(appFormIds), "application/octet-stream", "Общий расчет часов.xlsx");
    }
    
    [HttpGet(RouteConstants.REPORTS_DISTRIBUTION)]
    public ActionResult GetDistributionReport(HashSet<Guid> appFormIds)
    {
        return File(_distributionReportBuilder.BuildReport(appFormIds), "application/octet-stream", "карточки учебных поручений.xlsx");
    }
    
    [HttpGet(RouteConstants.REPORTS_STUDY_ASSIGNMENT)]
    public ActionResult GetStudyAssignmentReport(HashSet<Guid> appFormIds)
    {
        return File(_studyAssignmentReportBuilder.BuildReport(appFormIds), "application/octet-stream", "отчет о распределении учебных поручений.xlsx");
    }
}