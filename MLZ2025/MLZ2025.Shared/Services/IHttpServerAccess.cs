using MLZ2025.Core.Model;
using MLZ2025.Shared.Model;

namespace MLZ2025.Shared.Services;

public interface IHttpServerAccess
{
    Task<IList<ServerAddress>> GetAddressesAsync();
    Task SaveAddressAsync(ViewAddress address); // Neue Methode zum Speichern
    Task DeleteAddressAsync(ViewAddress address); // Löschen per HTTP
}
