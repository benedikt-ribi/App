using LZ1.Core;
using LZ1.Core.Services;

namespace LZ1.App;

public partial class MainPage : ContentPage
{
    private readonly ICounterService _counterService;

    public MainPage(ICounterService counterService)
    {
        _counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));

        InitializeComponent();
    }

    private async void OnIncrementClicked(object? sender, EventArgs e)
    {
        if (await _counterService.TryIncrement())
        {
            UpdateLabel();
        }
    }

    private async void OnDecrementClicked(object? sender, EventArgs e)
    {
        if (await _counterService.TryDecrement())
        {
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        CounterLabel.Text = _counterService.GetLabel();
        SemanticScreenReader.Announce(CounterLabel.Text);
    }
}