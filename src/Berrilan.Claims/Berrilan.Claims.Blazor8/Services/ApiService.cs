using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Berrilan.Claims.Blazor8.Services;

public class ApiService(HttpClient httpClient)
{
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<GetMeResponse> GetMe()
    {
        return await httpClient.GetFromJsonAsync<GetMeResponse>("me", jsonOptions);
    }
}
