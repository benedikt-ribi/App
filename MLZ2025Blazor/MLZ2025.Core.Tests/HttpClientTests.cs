using System.Net;
using MLZ2025.Core.Model;
using MLZ2025.Core.Services;
using MLZ2025.Shared.Services;
using Newtonsoft.Json;

namespace MLZ2025.Core.Tests;

[TestFixture]
public class HttpClientTests : TestsBase
{
    [Test]
    public async Task TestGetAddresses()
    {
        var serviceProvider = CreateServiceProvider();
        var serverAccess = serviceProvider.GetRequiredService<IHttpServerAccess>();
        var addresses = await serverAccess.GetAddressesAsync();

        Assert.That(addresses, Is.Not.Null);
        Assert.That(addresses.Count, Is.EqualTo(2));
    }
}

public class AddressServerAccess : HttpServerAccess
{
    public AddressServerAccess(HttpClient client)
        : base(client)
    {
    }
}
