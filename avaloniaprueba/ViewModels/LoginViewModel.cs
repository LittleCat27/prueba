using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using avaloniaprueba.Services;

namespace avaloniaprueba.ViewModels;

public partial class LoginViewModel(ApiClient apiClient, Action showRegister, Func<Task> showLogs) : ViewModelBase
{
    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _statusMessage = "";

    [RelayCommand]
    private void ShowRegister() => showRegister();

    [RelayCommand]
    private async Task LoginAsync()
    {
        StatusMessage = "";
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Ingresá el usuario y la contraseña.";
            return;
        }
        if (Username.Trim().Length > 100 || Password.Length > 128)
        {
            StatusMessage = "El usuario admite hasta 100 caracteres y la contraseña hasta 128.";
            return;
        }
        try
        {
            await apiClient.LoginAsync(Username.Trim(), Password);
            Password = "";
            await showLogs();
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            StatusMessage = AuthErrors.GetMessage(exception);
        }
    }
}