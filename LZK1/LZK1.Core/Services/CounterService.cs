namespace LZ1.Core.Services;

internal class CounterService : ICounterService
{
    private const string ConfirmIncrementMessage = "Are you sure you want to increment?";
    private const string ConfirmDecrementMessage = "Are you sure you want to decrement?";

    private readonly ICounterState _state;
    private readonly IDialogService _dialogService;

    public CounterService(ICounterState state, IDialogService dialogService)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    /// <inheritdoc/>
    public void Increment()
    {
        _state.Increment();
    }

    public void Decrement()
    {
        _state.Decrement();
    }

    /// <inheritdoc/>
    public async Task<bool> TryIncrement()
    {
        var result = await _dialogService.AskAsync(ConfirmIncrementMessage);

        if (result)
        {
            Increment();
        }

        return result;
    }

    public async Task<bool> TryDecrement()
    {
        var result = await _dialogService.AskAsync(ConfirmDecrementMessage);

        if (result)
        {
            Decrement();
        }

        return result;
    }

    /// <inheritdoc/>
    public string GetLabel()
    {
        var suffix = _state.Count == 1 ? string.Empty : "s";

        return $"Clicked {_state.Count} time{suffix}";
    }
}