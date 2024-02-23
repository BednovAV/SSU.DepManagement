using System.Net.Http.Json;
using Models.View;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Client.Extensions;

public static class HttpClientFacultyExtension
{
    private const string FACULTY_URL = RouteConstants.FACULTY;

    public static async Task<IEnumerable<FacultyViewItem>> GetFacultiesAsync(this HttpClient http)
    {
        return await http.GetFromJsonAsync<IEnumerable<FacultyViewItem>>(FACULTY_URL);
    }
    
    public static async Task DeleteFacultyAsync(this HttpClient http, int id)
    {
        await http.DeleteAsync(FACULTY_URL + $"/{id}");
    }
    
    public static async Task CreateFacultyAsync(this HttpClient http, FacultyViewItem faculty)
    {
        await http.PostAsync(FACULTY_URL, JsonContent.Create(faculty));
    }
    
    public static async Task UpdateFacultyAsync(this HttpClient http, FacultyViewItem faculty)
    {
        await http.PutAsync(FACULTY_URL, JsonContent.Create(faculty));
    }
}