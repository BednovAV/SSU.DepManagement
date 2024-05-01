using System.Net.Http.Json;
using Models.View;
using SSU.DM.WebAssembly.Shared;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.WebAssembly.Client.Extensions;

public static class HttpClientAppFormExtension
{
    //string deleteAppFormUrl => Http.GetUrl(RouteConstants.IMPORT_REQUEST_DELETE_APP_FORM);
    private const string DELETE_URL = RouteConstants.IMPORT_REQUEST_DELETE_APP_FORM;

    //string getAppFormsUrl => Http.GetUrl(RouteConstants.IMPORT_REQUEST_GET_APP_FORMS);
    private const string GET_URL = RouteConstants.IMPORT_REQUEST_GET_APP_FORMS;
    private const string CREATE_FACULTY_LINK_URL = RouteConstants.IMPORT_REQUEST_FACULTY_LINK;
    private const string CREATE_TEACHER_LINK_URL = RouteConstants.IMPORT_REQUEST_TEACHER_LINK;
    private const string VALIDATE_TEACHER_LINK_URL = RouteConstants.IMPORT_REQUEST_TEACHER_LINK_VALIDATE;
    private const string ASSIGN_TEACHERS_LINK_URL = RouteConstants.IMPORT_REQUEST_ASSIGN_TEACHERS;


    public static async Task<HttpResponseMessage> DeleteAppFormAsync(this HttpClient http, Guid id)
    {
        return await http.PostAsync(DELETE_URL, JsonContent.Create(new { appFormId = id }));
    }

    public static async Task<IEnumerable<ApplicationFormViewItem>> GetAppFormsAsync(this HttpClient http)
    {
        return (await http.GetFromJsonAsync<IEnumerable<ApplicationFormViewItem>>(GET_URL)).ToList();
    }

    public static async Task<HttpResponseMessage> CreateAppFormFacultyLinkAsync(this HttpClient http,
        CreateAppFormFacultyLinkRequest request)
    {
        return await http.PostAsync(CREATE_FACULTY_LINK_URL, JsonContent.Create(request));
    }

    public static async Task<CreateTeacherLinkResponse> CreateRequestTeacherLinkAsync(this HttpClient http,
        CreateRequestTeacherLinkRequest request)
    {
        var response = await http.PostAsync(CREATE_TEACHER_LINK_URL, JsonContent.Create(request));
        return await response.Content.ReadFromJsonAsync<CreateTeacherLinkResponse>();
    }

    public static async Task<bool> ValidateRequestTeacherLinkAsync(this HttpClient http,
        long teacherId)
    {
        return (await http.GetFromJsonAsync<ValidateRequestTeacherLinkResponse>(
            VALIDATE_TEACHER_LINK_URL + $"/{teacherId}")).Result;
    }

    public static async Task AssignTeachersAsync(this HttpClient http, List<Guid> appFormIds)
    {
        await http.PostAsync(ASSIGN_TEACHERS_LINK_URL, JsonContent.Create(appFormIds));
    }
}