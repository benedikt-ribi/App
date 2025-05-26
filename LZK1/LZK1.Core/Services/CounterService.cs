namespace LZ1.Core.Services;

internal class CounterService : ICounterService
{
    private const string ConfirmationMessage = "Are you sure you want to increment?";

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
        // This method is not part of the ICounterService interface but can be added if needed.
        // Currently, the service only supports incrementing the count.
        throw new NotImplementedException("This test is not implemented.");
    }

    /// <inheritdoc/>
    public async Task<bool> TryIncrement()
    {
        var result = await _dialogService.AskAsync(ConfirmationMessage);

        if (result)
        {
            Increment();
        }

        return result;
    }

    public async Task<bool> TryDecrement()
    {
        var result = await _dialogService.AskAsync(ConfirmationMessage);

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