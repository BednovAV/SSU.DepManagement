using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.View;
using SSU.DM.LogicLayer.Interfaces.Competencies;
using SSU.DM.LogicLayer.Interfaces.Teachers;
using SSU.DM.WebAssembly.Shared;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.WebAssembly.Server.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class TeachersController : ControllerBase
{
    private readonly ITeacherLogic _teacherLogic;
    private readonly ICompetenceLogic _competenceLogic;

    public TeachersController(
        ITeacherLogic teacherLogic,
        ICompetenceLogic competenceLogic)
    {
        _teacherLogic = teacherLogic;
        _competenceLogic = competenceLogic;
    }
    
    [HttpGet(RouteConstants.TEACHER)]
    public ActionResult GetAllTeachers()
    {
        return Ok(_teacherLogic.GetAll().Reverse());
    }
    
    [HttpGet(RouteConstants.TEACHER_CAPACITY)]
    public ActionResult GetTeacherCapacities()
    {
        return Ok(_teacherLogic.GetTeacherCapacities());
    }
    
    [HttpPost(RouteConstants.TEACHER)]
    public ActionResult CreateTeacher([FromBody]TeacherViewItem request)
    {
        _teacherLogic.Create(request);
        return Ok();
    }
    
    [HttpPut(RouteConstants.TEACHER)]
    public ActionResult UpdateTeacher([FromBody]TeacherViewItem request)
    {
        _teacherLogic.Update(request);
        return Ok();
    }
    
    [HttpDelete(RouteConstants.TEACHER + "/{id:long}")]
    public ActionResult DeleteTeacher(long id)
    {
        _teacherLogic.Delete(id);
        return Ok();
    }
    
    [HttpGet(RouteConstants.TEACHER_COMPETENCE + "/{id:long}")]
    public ActionResult GetTeacherCompetencies(long id)
    {
        return Ok(_competenceLogic.GetTeacherCompetencies(id));
    }
    
    [HttpPost(RouteConstants.TEACHER_COMPETENCE)]
    public ActionResult SaveTeacherCompetencies([FromBody]SaveTeacherCompetenciesRequest request)
    {
        _competenceLogic.SaveTeacherCompetencies(request.TeacherId, request.Competencies);
        return Ok();
    }
}