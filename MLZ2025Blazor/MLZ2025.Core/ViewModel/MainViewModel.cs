using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MLZ2025.Core.Model;
using MLZ2025.Core.Services;
using MLZ2025.Shared.Model;
using MLZ2025.Shared.Services;

namespace MLZ2025.Core.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly IConnectivity _connectivity;
    private readonly IDialogService _dialogService;
    private readonly DataAccess<DatabaseAddress> _dataAccess;
    private readonly DataLoader _dataLoader;

    // TODO Use a custom object instead.
    [ObservableProperty] private ObservableCollection<DatabaseAddress> _items = [];

    [ObservableProperty] private string _firstName = "Bob";
    [ObservableProperty] private string _lastName = "Jones";
    [ObservableProperty] private string _zipCode = "13357";
    [ObservableProperty] private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Today);

    public MainViewModel(IConnectivity connectivity, IDialogService dialogService, DataAccess<DatabaseAddress> dataAccess, DataLoader dataLoader)
    {
        _connectivity = connectivity;
        _dialogService = dialogService;
        _dataAccess = dataAccess;
        _dataLoader = dataLoader;

        // TODO add a loading property

        // Task.Run(LoadAsync);
    }

    [RelayCommand]
    private async Task Load()
    {
        var addresses = await _dataLoader.GetDatabaseAddresses();

        Items = new ObservableCollection<DatabaseAddress>(addresses);
    }

    [RelayCommand]
    private async Task Add()
    {
        // TODO Validate other fields (birthday, ZipCode, etc.)

        if (await ValidateText(FirstName) && await ValidateText(LastName))
        {
            var d = new DatabaseAddress
            {
                FirstName = FirstName,
                LastName = LastName,
                ZipCode = ZipCode,
                Birthday = Birthday.ToDateTime(TimeOnly.MinValue)
            };

            Items.Add(d);
            _dataAccess.Insert(d);
        }
    }

    [RelayCommand]
    private async Task Delete(DatabaseAddress item)
    {
        if (!Items.Remove(item))
        {
            Debug.WriteLine($"Cannot remove {item} because it is not in the list.");
        }
    }

    [RelayCommand]
    private async Task Select(DatabaseAddress item)
    {
        // TODO Use the dictionary instead.
        // Figure out how to test the Shell <https://software-engineering-corner.zuehlke.com/how-to-test-a-net-maui-app-part-1#heading-testing-of-a-view-model>
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?{nameof(DetailViewModel.Text)}={item}");
    }

    private async Task<bool> ValidateText(string text)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await _dialogService.ShowErrorMessage("No Internet. Please check your internet connection.");

            return false;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            await ShowEmptyTextErrorMessage();

            // TODO Use a logger instead.
            Debug.WriteLine("Text is empty");

            return false;
        }

        return true;
    }

    private async Task ShowEmptyTextErrorMessage()
    {
        await _dialogService.ShowErrorMessage("Please enter a text");
    }
}
