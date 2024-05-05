using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.View;
using SSU.DM.LogicLayer.Interfaces.JobTitle;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

[Authorize(Roles = Roles.ADMIN)]
public class JobTitleController : ControllerBase
{
    private readonly IJobTitleLogic _jobTitleLogic;

    public JobTitleController(IJobTitleLogic jobTitleLogic)
    {
        _jobTitleLogic = jobTitleLogic;
    }
    
    [HttpGet(RouteConstants.JOB_TITLE)]
    public ActionResult GetAll()
    {
        return Ok(_jobTitleLogic.GetAll().Reverse());
    }
    
    [HttpPost(RouteConstants.JOB_TITLE)]
    public ActionResult Add([FromBody]JobTitleViewItem jobTitle)
    {
        try
        {
            _jobTitleLogic.Add(jobTitle);

        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
        return Ok();
    }
    
    [HttpPatch(RouteConstants.JOB_TITLE)]
    public ActionResult Update([FromBody]JobTitleViewItem jobTitle)
    {
        try
        {
            _jobTitleLogic.Update(jobTitle);

        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
        return Ok();
    }
    
    [HttpPatch(RouteConstants.JOB_TITLE + "/{id:long}")]
    public ActionResult Delete(long id)
    {
        try
        {
            _jobTitleLogic.Delete(id);

        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
        return Ok();
    }
}