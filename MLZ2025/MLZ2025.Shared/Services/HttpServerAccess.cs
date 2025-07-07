using MLZ2025.Core.Model;
using Newtonsoft.Json;
using MLZ2025.Shared.Model;
using System.Text;

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

    public async Task SaveAddressAsync(ViewAddress address)
    {
        var json = JsonConvert.SerializeObject(address);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/addresses/", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAddressAsync(ViewAddress address)
    {
        // Annahme: Löschen erfolgt über eindeutige Kombination von Feldern (z.B. Name oder Id)
        // Hier als Beispiel mit FirstName und LastName
        var response = await _client.DeleteAsync($"/addresses?firstName={Uri.EscapeDataString(address.FirstName)}&lastName={Uri.EscapeDataString(address.LastName)}");
        response.EnsureSuccessStatusCode();
    }
}
