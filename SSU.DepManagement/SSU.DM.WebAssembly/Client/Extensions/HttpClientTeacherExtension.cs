using System.Net.Http.Json;
using Models.Request;
using Models.View;
using SSU.DM.WebAssembly.Shared;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.WebAssembly.Client.Extensions;

public static class HttpClientTeacherExtension
{
    private const string TEACHER_URL = RouteConstants.TEACHER;
    private const string TEACHER_CAPACITIES_URL = RouteConstants.TEACHER_CAPACITY;
    private const string TEACHER_COMPETENCIES_URL = RouteConstants.TEACHER_COMPETENCE;

    public static async Task<IEnumerable<TeacherViewItem>> GetTeachersAsync(this HttpClient http)
    {
        return await http.GetFromJsonAsync<IEnumerable<TeacherViewItem>>(TEACHER_URL);
    }
    
    public static async Task<IEnumerable<TeacherCapacitiesViewItem>> GetTeacherCapacitiesAsync(this HttpClient http)
    {
        return await http.GetFromJsonAsync<IEnumerable<TeacherCapacitiesViewItem>>(TEACHER_CAPACITIES_URL);
    }
    
    public static async Task UpdateTeacherCapacitiesAsync(this HttpClient http, UpdateTeacherCapacityRequest request)
    {
        await http.PutAsync(TEACHER_CAPACITIES_URL, JsonContent.Create(request));
    }
    
    public static async Task<IDictionary<LessonForm, IReadOnlyList<TeacherCompetenciesViewItem>>> GetTeacherCompetenciesAsync(
        this HttpClient http, long teacherId)
    {
        return await http.GetFromJsonAsync<IDictionary<LessonForm, IReadOnlyList<TeacherCompetenciesViewItem>>>(
            TEACHER_COMPETENCIES_URL + $"/{teacherId}");
    }
    
    public static async Task SaveTeacherCompetenciesAsync(this HttpClient http, SaveTeacherCompetenciesRequest request)
    {
        await http.PostAsync(TEACHER_COMPETENCIES_URL, JsonContent.Create(request));
    }
    
    public static async Task DeleteTeacherAsync(this HttpClient http, long id)
    {
        await http.DeleteAsync(TEACHER_URL + $"/{id}");
    }
    
    public static async Task CreateTeacherAsync(this HttpClient http, TeacherViewItem teacher)
    {
        await http.PostAsync(TEACHER_URL, JsonContent.Create(teacher));
    }
    
    public static async Task UpdateTeacherAsync(this HttpClient http, TeacherViewItem teacher)
    {
        await http.PutAsync(TEACHER_URL, JsonContent.Create(teacher));
    }
}