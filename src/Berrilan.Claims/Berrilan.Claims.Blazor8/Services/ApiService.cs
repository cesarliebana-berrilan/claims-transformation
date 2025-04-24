using System.Net.Http.Json;

namespace Berrilan.Claims.Blazor8.Services;

public class ApiService(HttpClient httpClient)
{
    public async Task<GetMeResponse> GetMe()
    {
        return await httpClient.GetFromJsonAsync<GetMeResponse>("me");
    }
}
