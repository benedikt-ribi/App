using MLZ2025.Core.Model;
using Newtonsoft.Json;

namespace MLZ2025.Shared.Services;

public class HttpServerAccess : IHttpServerAccess
{
    private readonly HttpClient _client;

    public HttpServerAccess(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("http://localhost:3000");
    }

    public async Task<IList<ServerAddress>> GetAddressesAsync()
    {
        var response = await _client.GetAsync("/addresses/");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        // TODO Use System.Text.Json instead of Newtonsoft.Json
        var result = JsonConvert.DeserializeObject<IList<ServerAddress>>(content);

        if (result == null)
        {
            throw new InvalidOperationException("Could not get addresses from server.");
        }

        return result;
    }
}
