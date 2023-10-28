using Microsoft.AspNetCore.Mvc;
using Models.View;
using SSU.DM.LogicLayer.Interfaces.Faculty;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class FacultyController : ControllerBase
{
    private readonly IFacultyLogic _facultyLogic;

    public FacultyController(IFacultyLogic facultyLogic)
    {
        _facultyLogic = facultyLogic;
    }

    [HttpGet(RouteConstants.FACULTY)]
    public ActionResult GetAllFaculties()
    {
        return Ok(_facultyLogic.GetAll().Reverse());
    }
    
    [HttpPost(RouteConstants.FACULTY)]
    public ActionResult AddFaculty([FromBody]FacultyViewItem faculty)
    {
        _facultyLogic.AddFaculty(faculty);
        return Ok();
    }
    
    [HttpDelete(RouteConstants.FACULTY + "/{id:int}")]
    public ActionResult DeleteFaculty(int id)
    {
        _facultyLogic.DeleteFaculty(id);
        return Ok();
    }
    
    [HttpPut(RouteConstants.FACULTY)]
    public ActionResult UpdateFaculty([FromBody]FacultyViewItem faculty)
    {
        _facultyLogic.UpdateFaculty(faculty);
        return Ok();
    }
}