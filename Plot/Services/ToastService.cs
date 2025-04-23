namespace Plot.Services;

public class ToastMessage
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
    public int Duration { get; set; } = 3000;
}

public class ToastService
{
    public event Action? OnToastsUpdated;

    private readonly Queue<ToastMessage> _toasts = new();

    public IReadOnlyList<ToastMessage> Toasts => _toasts.ToList().AsReadOnly();

    public void ShowToast(string message, string type = "info", int duration = 3000)
    {
        var toast = new ToastMessage
        {
            Message = message,
            Type = type,
            Duration = duration,
        };

        _toasts.Enqueue(toast);

        OnToastsUpdated?.Invoke();
    }

    public void PopToast()
    {
        _toasts.Dequeue();

        OnToastsUpdated?.Invoke();
    }

    public void ShowSuccess(string message, int duration = 3000) => ShowToast(message, "success", duration);
    public void ShowError(string message, int duration = 3000) => ShowToast(message, "danger", duration);
    public void ShowWarning(string message, int duration = 3000) => ShowToast(message, "warning", duration);
    public void ShowInfo(string message, int duration = 3000) => ShowToast(message, "info", duration);
}