using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyShop.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        return true;
    }

    private string _message = string.Empty;
    private bool _isError;

    public string Message { get => _message; set => Set(ref _message, value); }
    public bool IsError { get => _isError; set => Set(ref _isError, value); }

    protected void ShowSuccess(string text) { IsError = false; Message = text; }
    protected void ShowError(string text) { IsError = true; Message = text; }
}
