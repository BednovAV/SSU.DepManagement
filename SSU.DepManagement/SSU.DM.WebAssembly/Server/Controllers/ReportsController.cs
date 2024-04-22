using Microsoft.AspNetCore.Mvc;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.WebAssembly.Shared;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class ReportsController : ControllerBase
{
    private readonly ICalculationOfHoursBuilder _calculationOfHoursBuilder;
    private readonly IDistributionReportBuilder _distributionReportBuilder;

    public ReportsController(
        ICalculationOfHoursBuilder calculationOfHoursBuilder,
        IDistributionReportBuilder distributionReportBuilder)
    {
        _calculationOfHoursBuilder = calculationOfHoursBuilder;
        _distributionReportBuilder = distributionReportBuilder;
    }

    [HttpGet(RouteConstants.REPORTS_CALCULATION_OF_HOURS)]
    public ActionResult GetCalculationOfHoursReport(HashSet<Guid> appFormIds)
    {
        return File(_calculationOfHoursBuilder.BuildReport(appFormIds), "application/octet-stream", "Общий расчет часов.xlsx");
    }
    
    [HttpGet(RouteConstants.REPORTS_DISTRIBUTION)]
    public ActionResult GetDistributionReport(HashSet<Guid> appFormIds)
    {
        return File(_distributionReportBuilder.BuildReport(appFormIds), "application/octet-stream", "отчет о распределении учебных поручений.xlsx");
    }
}