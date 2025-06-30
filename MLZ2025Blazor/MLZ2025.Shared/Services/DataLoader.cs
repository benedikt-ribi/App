using MLZ2025.Shared.Model;

namespace MLZ2025.Shared.Services;

public class DataLoader
{
    private readonly DataAccess<DatabaseAddress> _dataAccess;
    private readonly IHttpServerAccess _httpServerAccess;

    public DataLoader(DataAccess<DatabaseAddress> dataAccess, IHttpServerAccess httpServerAccess)
    {
        _dataAccess = dataAccess;
        _httpServerAccess = httpServerAccess;
    }

    public async Task<List<DatabaseAddress>> GetDatabaseAddresses()
    {
        var addresses = _dataAccess.Table().ToList();

        if (addresses.Count == 0)
        {
            var serverAddresses = await _httpServerAccess.GetAddressesAsync();
            addresses = serverAddresses.Select(address => new DatabaseAddress { FirstName = address.FirstName }).ToList();
        }

        return addresses;
    }
}
