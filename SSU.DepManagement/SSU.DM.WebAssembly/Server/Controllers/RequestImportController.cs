using Microsoft.AspNetCore.Mvc;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Server.Controllers;

public class RequestImportController : ControllerBase
{
    private readonly IRequestAccessor _requestAccessor;
    private readonly IRequestEditor _requestEditor;

    public RequestImportController(
        IRequestAccessor requestAccessor,
        IRequestEditor requestEditor)
    {
        _requestAccessor = requestAccessor;
        _requestEditor = requestEditor;
    }

    [HttpGet(RouteConstants.IMPORT_REQUEST_GET_APP_FORMS)]
    public ActionResult GetAppForms()
    {
        return Ok(_requestAccessor.GetApplicationForms());
    }
    
    [HttpGet(RouteConstants.IMPORT_REQUEST_GET_FILE)]
    public ActionResult GetFile(string key = null)
    {
        var (name, bytes) = _requestAccessor.GetFile(key);
        return File(bytes, "application/octet-stream", name);
    }
    
    [HttpPost(RouteConstants.IMPORT_REQUEST_UPLOAD_REQUEST)]
    public async Task<ActionResult> UploadRequest(IFormFile file)
    {
        return Ok(await _requestEditor.UploadFromStream(file.FileName, file.OpenReadStream()));
    }
    
    [HttpPost(RouteConstants.IMPORT_REQUEST_DELETE_APP_FORM)]
    public async Task<ActionResult> DeleteAppForm([FromBody]MyRequest request)
    {
        // if (!Guid.TryParse(appFormId, out var guidId))
        //     return StatusCode(StatusCodes.Status400BadRequest);
            
        var deleteResult = await _requestEditor.DeleteAsync(request.AppFormId);
        return deleteResult ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        
    }
    public class MyRequest
    {
        public Guid AppFormId { get; set; }
    }
}
