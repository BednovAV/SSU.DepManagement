using System.Net.Http.Json;
using Models.View;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Client.Extensions;

public static class HttpClientJobTitleExtension
{
    private const string JOB_TITLE_URL = RouteConstants.JOB_TITLE;
    
    public static async Task<IEnumerable<JobTitleViewItem>> GetJobTitlesAsync(this HttpClient http)
    {
        return await http.GetFromJsonAsync<IEnumerable<JobTitleViewItem>>(JOB_TITLE_URL);
    }
    
    public static async Task DeleteJobTitleAsync(this HttpClient http, long id)
    {
        await http.DeleteAsync(JOB_TITLE_URL + $"/{id}");
    }
    
    public static async Task CreateJobTitleAsync(this HttpClient http, JobTitleViewItem jobTitle)
    {
        await http.PostAsync(JOB_TITLE_URL, JsonContent.Create(jobTitle));
    }
    
    public static async Task UpdateJobTitleAsync(this HttpClient http, JobTitleViewItem jobTitle)
    {
        await http.PatchAsync(JOB_TITLE_URL, JsonContent.Create(jobTitle));
    }
}