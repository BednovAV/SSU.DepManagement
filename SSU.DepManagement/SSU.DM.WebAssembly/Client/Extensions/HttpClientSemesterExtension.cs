using System.Net.Http.Json;
using Models.View;
using SSU.DM.WebAssembly.Shared;

namespace SSU.DM.WebAssembly.Client.Extensions;

public static class HttpClientSemesterExtension
{
    private const string SEMESTER_URL = RouteConstants.SEMESTER;

    public static async Task<IReadOnlyList<SemesterViewItem>> GetSemestersAsync(this HttpClient http)
    {
        return (await http.GetFromJsonAsync<IEnumerable<SemesterViewItem>>(SEMESTER_URL)).ToList();
    }
}