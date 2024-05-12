using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class ReportsController : ControllerBase
{
    private readonly IReportBuilder _reportBuilder;

    public ReportsController(
        IReportBuilder reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    [HttpGet(RouteConstants.REPORTS_GENERATE)]
    public ActionResult GenerateReports(GenerateReportsRequest request)
    {
        return File(_reportBuilder
            .BuildReport(request), "application/octet-stream", "нагрузка кафедры.xlsx");
    }
}