using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
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
    private readonly IHttpServerAccess _httpServerAccess; // Hinzugefügt

    [ObservableProperty] private ViewAddressCollection _items = new();
    [ObservableProperty] private string _firstName = "Bob";
    [ObservableProperty] private string _lastName = "Jones";
    [ObservableProperty] private string _zipCode = "13357";
    [ObservableProperty] private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Today);
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _email = "Bob.Jones@bluewin.ch";
    [ObservableProperty] private string _phone = "079 569 65 89";

    public MainViewModel(IConnectivity connectivity, IDialogService dialogService, DataAccess<DatabaseAddress> dataAccess, DataLoader dataLoader, IHttpServerAccess httpServerAccess)
    {
        _connectivity = connectivity;
        _dialogService = dialogService;
        _dataAccess = dataAccess;
        _dataLoader = dataLoader;
        _httpServerAccess = httpServerAccess; // Hinzugefügt

        IsLoading = false;
        // Task.Run(LoadAsync);
    }

    [RelayCommand]
    private async Task Load()
    {
        IsLoading = true;
        try
        {
            var addresses = await _dataLoader.GetDatabaseAddresses();
            Items = new ViewAddressCollection();
            foreach (var addr in addresses.Select(ViewAddress.FromDatabaseAddress))
                Items.Add(addr);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Add()
    {
        var d = new ViewAddress
        {
            FirstName = FirstName,
            LastName = LastName,
            ZipCode = ZipCode,
            Birthday = Birthday.ToDateTime(TimeOnly.MinValue),
            Email = Email,
            Phone = Phone
        };

        // DataAnnotations-Validierung
        var context = new ValidationContext(d);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(d, context, results, true);

        if (!isValid)
        {
            var errorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            await _dialogService.ShowErrorMessage(errorMessage);
            return;
        }

        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await _dialogService.ShowErrorMessage("No Internet. Please check your internet connection.");
            return;
        }

        Items.Add(d);
        _dataAccess.Insert(ViewAddress.ToDatabaseAddress(d));
        await _httpServerAccess.SaveAddressAsync(d); // Speichern per HTTP
    }

    [RelayCommand]
    private async Task Delete(ViewAddress item)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await _dialogService.ShowErrorMessage("No Internet. Please check your internet connection.");
            return;
        }

        // Validierung: Pflichtfelder prüfen
        var context = new ValidationContext(item);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(item, context, results, true);

        if (!isValid)
        {
            var errorMessage = string.Join("\n", results.Select(r => r.ErrorMessage));
            await _dialogService.ShowErrorMessage(errorMessage);
            return;
        }

        if (!Items.Remove(item))
        {
            Debug.WriteLine($"Cannot remove {item} because it is not in the list.");
            return;
        }

        // Lokal löschen
        _dataAccess.Delete(ViewAddress.ToDatabaseAddress(item));
        await _httpServerAccess.DeleteAddressAsync(item); // Per HTTP löschen

        // Bestätigung anzeigen
        await _dialogService.ShowErrorMessage("Eintrag gelöscht.");
    }

    [RelayCommand]
    private async Task Select(ViewAddress item)
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
