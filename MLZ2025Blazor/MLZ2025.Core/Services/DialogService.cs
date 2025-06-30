using Microsoft.Maui.Controls;

namespace MLZ2025.Core.Services;

public class DialogService : IDialogService
{
    public async Task ShowErrorMessage(string message)
    {
        await Shell.Current.DisplayAlert("Error", message, "OK");
    }
}
