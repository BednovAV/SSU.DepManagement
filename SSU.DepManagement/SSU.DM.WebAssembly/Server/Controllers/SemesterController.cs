using Microsoft.AspNetCore.Mvc;
using Models.View;
using SSU.DM.LogicLayer.Interfaces.Faculty;
using SSU.DM.LogicLayer.Interfaces.Semesters;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class SemesterController : ControllerBase
{
    private readonly ISemesterLogic _semesterLogic;

    public SemesterController(ISemesterLogic semesterLogic)
    {
        _semesterLogic = semesterLogic;
    }

    [HttpGet(RouteConstants.SEMESTER)]
    public ActionResult GetAllSemesters()
    {
        return Ok(_semesterLogic.GetAll());
    }
}