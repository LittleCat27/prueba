using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using avaloniaprueba.Services;
using System.Text.RegularExpressions;
namespace avaloniaprueba.ViewModels;

public partial class RegisterViewModel(ApiClient apiClient, Action showLogin) : ViewModelBase
{
    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _mail = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _statusMessage = "";

    [RelayCommand]
    private void ShowLogin() => showLogin();

    [RelayCommand]
    private async Task RegisterAsync()
    {
        StatusMessage = "";
        if (string.IsNullOrWhiteSpace(Username) || Username.Trim().Length is < 3 or > 100)
        {
            StatusMessage = "El usuario debe tener entre 3 y 100 caracteres.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Mail) || Mail.Trim().Length > 255 ||
            !Regex.IsMatch(Mail.Trim(), @"^[\w\.]+@[\w\.]+\.\w+$") ) //Regex simple para validar un correo electrónico
        {
            StatusMessage = "Ingresá un correo electrónico válido de hasta 255 caracteres.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password) || Password.Length is < 8 or > 128)
        {
            StatusMessage = "La contraseña debe tener entre 8 y 128 caracteres.";
            return;
        }
        try
        {
            var user = await apiClient.RegisterAsync(Username.Trim(), Mail.Trim(), Password);
            Password = "";
            StatusMessage = $"Cuenta creada: {user.Username} (ID {user.Id}).";
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            StatusMessage = AuthErrors.GetMessage(exception);
        }
    }
}