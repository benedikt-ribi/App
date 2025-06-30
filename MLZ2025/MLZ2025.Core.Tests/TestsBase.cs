using MLZ2025.Core.Services;
using MLZ2025.Shared.Services;

namespace MLZ2025.Core.Tests;

public class TestsBase
{
    protected readonly TestDialogService _testDialogService = new();
    protected readonly TestConnectivity _testConnectivity = new();

    protected ServiceProvider CreateServiceProvider()
    {
        return CreateServiceCollection().BuildServiceProvider();
    }

    protected IServiceCollection CreateServiceCollection()
    {
        _testConnectivity.NetworkAccess = NetworkAccess.Internet;
        _testDialogService.LastMessage = string.Empty;

        return new ServiceCollection()
            .AddCoreServices()
            .AddSingleton<IDialogService>(_testDialogService)
            .AddSingleton<IConnectivity>(_testConnectivity)
            .AddSingleton(new DataAccessSettings() { Filename = "test.db" });
    }

    protected class TestDialogService : IDialogService
    {
        public Task ShowErrorMessage(string message)
        {
            LastMessage = message;

            return Task.CompletedTask;
        }

        public string LastMessage { get; set; } = string.Empty;
    }

    protected class TestConnectivity : IConnectivity
    {
        public NetworkAccess NetworkAccess { get; set; } = NetworkAccess.Internet;

        public event EventHandler<ConnectivityChangedEventArgs>? ConnectivityChanged;

        public IEnumerable<ConnectionProfile> ConnectionProfiles => new List<ConnectionProfile>
        {
            ConnectionProfile.WiFi,
            ConnectionProfile.Cellular
        };
    }
}
