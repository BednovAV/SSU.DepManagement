using Microsoft.AspNetCore.Mvc;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class ReportsController : ControllerBase
{
    private readonly ICalculationOfHoursBuilder _calculationOfHoursBuilder;

    public ReportsController(ICalculationOfHoursBuilder calculationOfHoursBuilder)
    {
        _calculationOfHoursBuilder = calculationOfHoursBuilder;
    }

    [HttpGet(RouteConstants.REPORTS_CALCULATION_OF_HOURS)]
    public ActionResult GetCalculationOfHoursReport(HashSet<Guid> appFormIds)
    {
        return File(_calculationOfHoursBuilder.BuildReport(appFormIds), "application/octet-stream", "Общий расчет часов.xlsx");
    }
}