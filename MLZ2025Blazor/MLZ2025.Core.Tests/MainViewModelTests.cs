using MLZ2025.Core.Model;
using MLZ2025.Core.Services;
using MLZ2025.Core.ViewModel;
using MLZ2025.Shared.Model;
using MLZ2025.Shared.Services;

namespace MLZ2025.Core.Tests;

public class MainViewModelTests : TestsBase
{
    private static readonly string?[] EmptyTexts = ["", " ", "\t", "\r", "   ", null, "\n"];

    [TestCaseSource(nameof(EmptyTexts))]
    public void TestCannotAddEmptyText(string? text)
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();
        viewModel.FirstName = text;

        viewModel.AddCommand.Execute(null);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a text"));
    }

    [Test]
    public void TestInitializedWithExistingData()
    {
        var serviceProvider = CreateServiceCollection()
            .AddTransient<IHttpServerAccess, TestHttpServerAccess>()
            .BuildServiceProvider();
        using var dataAccess = serviceProvider.GetRequiredService<DataAccess<DatabaseAddress>>();

        var address = new DatabaseAddress { FirstName = "Bob", LastName = "Last Name" };
        List<string> expectedItems = [address.FirstName];

        dataAccess.DeleteAll();
        dataAccess.Insert(address);

        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        Assert.That(viewModel.Items, Is.EquivalentTo(expectedItems));
    }

    [Test]
    public void TestInitializedWithNoExistingData()
    {
        var serviceProvider = CreateServiceCollection()
            .AddTransient<IHttpServerAccess, TestHttpServerAccess>()
            .BuildServiceProvider();
        using var dataAccess = serviceProvider.GetRequiredService<DataAccess<DatabaseAddress>>();

        var expectedAddress = new DatabaseAddress { FirstName = "Max", LastName = "Mustermann" };
        List<string> expectedItems = [expectedAddress.FirstName];

        dataAccess.DeleteAll();

        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        Assert.That(viewModel.Items, Is.EquivalentTo(expectedItems));
    }

    [Test]
    public void TestCannotAddWithoutInternet()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();
        _testConnectivity.NetworkAccess = NetworkAccess.None;
        viewModel.FirstName = "Foo";

        viewModel.AddCommand.Execute(null);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("No Internet. Please check your internet connection."));
    }

    [TestCaseSource(nameof(EmptyTexts))]
    public void TestCannotSelectEmptyText(string? text)
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        viewModel.SelectCommand.Execute(text);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a text"));
    }

    [TestCaseSource(nameof(EmptyTexts))]
    public void TestCannotDeleteEmptyText(string? text)
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        viewModel.DeleteCommand.Execute(text);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a text"));
    }

    [Test]
    public void TestCannotDeleteWithoutInternet()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();
        _testConnectivity.NetworkAccess = NetworkAccess.None;

        var item = viewModel.Items.Last();

        viewModel.DeleteCommand.Execute(item);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("No Internet. Please check your internet connection."));
    }

    [Test]
    public void TestCannotSelectWithoutInternet()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();
        _testConnectivity.NetworkAccess = NetworkAccess.None;

        var item = viewModel.Items.Last();

        viewModel.SelectCommand.Execute(item);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("No Internet. Please check your internet connection."));
    }

    [Test]
    public void TestAddItem()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();
        viewModel.FirstName = "Item 1";

        viewModel.AddCommand.Execute(null);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo(""));
        Assert.That(viewModel.Items.Last(), Is.EqualTo(null));
    }

    [Test]
    public void TestDeleteItem()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        var item = viewModel.Items.Last();

        viewModel.DeleteCommand.Execute(item);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo(""));
        Assert.That(viewModel.Items, Does.Not.Contain(item));
    }

    [Test]
    public void TestSelectItem()
    {
        Application.Current = new App();

        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        var item = viewModel.Items.Last();

        viewModel.SelectCommand.Execute(item);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo(""));
        Assert.That(viewModel.FirstName, Is.EqualTo(item.FirstName));
    }

    private class TestHttpServerAccess : IHttpServerAccess
    {
        public Task<IList<ServerAddress>> GetAddressesAsync()
        {
            IList<ServerAddress> result = [new()
            {
                Id = "1",
                FirstName = "Max",
                LastName = "Mustermann"
            }];

            return Task.FromResult(result);
        }
    }
}
