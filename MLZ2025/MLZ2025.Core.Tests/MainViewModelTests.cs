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
        viewModel.FirstName = text ?? string.Empty;

        viewModel.AddCommand.Execute(null);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a first name"));
    }

    [Test]
    public void TestInitializedWithExistingData()
    {
        var serviceProvider = CreateServiceCollection()
            .AddTransient<IHttpServerAccess, TestHttpServerAccess>()
            .BuildServiceProvider();
        using var dataAccess = serviceProvider.GetRequiredService<DataAccess<DatabaseAddress>>();

        var address = new DatabaseAddress
        {
            FirstName = "Bob",
            LastName = "Last Name",
            ZipCode = "12345",
            Birthday = new DateTime(2000, 1, 1),
            Email = "bob@example.com",
            Phone = "0123456789"
        };
        List<string> expectedItems = [address.FirstName];

        dataAccess.DeleteAll();
        dataAccess.Insert(address);

        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        Assert.That(viewModel.Items.Select(i => i.FirstName), Is.EquivalentTo(expectedItems));
    }

    [Test]
    public void TestInitializedWithNoExistingData()
    {
        var serviceProvider = CreateServiceCollection()
            .AddTransient<IHttpServerAccess, TestHttpServerAccess>()
            .BuildServiceProvider();
        using var dataAccess = serviceProvider.GetRequiredService<DataAccess<DatabaseAddress>>();

        var expectedAddress = new DatabaseAddress
        {
            FirstName = "Max",
            LastName = "Mustermann",
            ZipCode = "54321",
            Birthday = new DateTime(1995, 5, 5),
            Email = "max@example.com",
            Phone = "9876543210"
        };
        List<string> expectedItems = [expectedAddress.FirstName];

        dataAccess.DeleteAll();

        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        Assert.That(viewModel.Items.Select(i => i.FirstName), Is.EquivalentTo(expectedItems));
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

        viewModel.SelectCommand.Execute(text ?? string.Empty);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a first name"));
    }

    [TestCaseSource(nameof(EmptyTexts))]
    public void TestCannotDeleteEmptyText(string? text)
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        viewModel.DeleteCommand.Execute(text ?? string.Empty);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Please enter a first name"));
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
        viewModel.LastName = "Last 1";
        viewModel.ZipCode = "11111";
        viewModel.Birthday = DateOnly.FromDateTime(new DateTime(1999, 9, 9));
        viewModel.Email = "item1@example.com";
        viewModel.Phone = "1234567890";

        viewModel.AddCommand.Execute(null);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo(""));
        Assert.That(viewModel.Items.Last().FirstName, Is.EqualTo("Item 1"));
    }

    [Test]
    public void TestDeleteItem()
    {
        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        // Add a new item to ensure there is something to delete
        viewModel.FirstName = "DeleteMe";
        viewModel.LastName = "ToDelete";
        viewModel.ZipCode = "99999";
        viewModel.Birthday = DateOnly.FromDateTime(new DateTime(2001, 1, 1));
        viewModel.Email = "deleteme@example.com";
        viewModel.Phone = "0000000000";
        viewModel.AddCommand.Execute(null);
        var item = viewModel.Items.Last();

        viewModel.DeleteCommand.Execute(item);

        Assert.That(_testDialogService.LastMessage, Is.EqualTo("Eintrag gelöscht."));
        Assert.That(viewModel.Items, Does.Not.Contain(item));
    }

    [Test]
    public void TestSelectItem()
    {
        Application.Current = new App();

        var serviceProvider = CreateServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<MainViewModel>();

        // Add a new item to ensure there is something to select
        viewModel.FirstName = "SelectMe";
        viewModel.LastName = "ToSelect";
        viewModel.ZipCode = "88888";
        viewModel.Birthday = DateOnly.FromDateTime(new DateTime(2002, 2, 2));
        viewModel.Email = "selectme@example.com";
        viewModel.Phone = "1111111111";
        viewModel.AddCommand.Execute(null);
        var item = viewModel.Items.Last();

        viewModel.SelectCommand.Execute(item);

        // No error message expected
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
                LastName = "Mustermann",
                ZipCode = "54321",
                Birthday = new DateTime(1995, 5, 5),
                Email = "max@example.com",
                Phone = "9876543210"
            }];

            return Task.FromResult(result);
        }

        public Task SaveAddressAsync(ViewAddress address)
        {
            // Test-Implementierung: tut nichts
            return Task.CompletedTask;
        }

        public Task DeleteAddressAsync(ViewAddress address)
        {
            // Test-Implementierung: tut nichts
            return Task.CompletedTask;
        }
    }
}
