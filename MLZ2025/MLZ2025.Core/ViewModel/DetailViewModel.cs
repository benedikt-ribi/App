using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace MLZ2025.Core.ViewModel;

[QueryProperty(nameof(Text), nameof(Text))]
public partial class DetailViewModel : ObservableObject
{
    [ObservableProperty]
    private string _text = "";

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}
