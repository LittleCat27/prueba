using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using avaloniaprueba.Services;

namespace avaloniaprueba.ViewModels;

public partial class LogsViewModel(ApiClient apiClient, Action showLogin) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<LoginLogRow> _items = [];

    [ObservableProperty]
    private string _statusMessage = "";

    [RelayCommand]
    private void Logout()
    {
        Items.Clear();
        showLogin();
    }

    private int _page = 1;
    private int _total;
    private bool _loading;
    private const int PageSize = 20;

    private bool CanPrevious() => !_loading && _page > 1;
    private bool CanNext() => !_loading && (long)_page * PageSize < _total;
    private bool CanRefresh() => !_loading;

    private async Task LoadAsync(int page)
    {
        if (_loading) return;
        _loading = true;
        UpdateCommands();
        StatusMessage = "Cargando...";
        try
        {
            var result = await apiClient.GetLoginLogsAsync(page, PageSize);
            _page = result.Page;
            _total = result.Total;
            Items = new ObservableCollection<LoginLogRow>();
            foreach (var item in result.Items)
                Items.Add(new LoginLogRow(item.Id, item.UsuarioId, item.Fecha, item.Success));
            StatusMessage = result.Total == 0 ? "No hay registros de login."
                : $"Página {_page}. Mostrando {Items.Count} de {_total} registros.";
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            StatusMessage = exception is TaskCanceledException
                ? "La API tardó demasiado en responder." : exception.Message;
        }
        finally
        {
            _loading = false;
            UpdateCommands();
        }
    }

    private void UpdateCommands()
    {
        RefreshCommand.NotifyCanExecuteChanged();
        PreviousCommand.NotifyCanExecuteChanged();
        NextCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private Task RefreshAsync() => LoadAsync(1);

    [RelayCommand(CanExecute = nameof(CanPrevious))]
    private Task PreviousAsync() => LoadAsync(_page - 1);

    [RelayCommand(CanExecute = nameof(CanNext))]
    private Task NextAsync() => LoadAsync(_page + 1);
}

public sealed class LoginLogRow(int id, int? usuarioId, DateTime? fecha, bool? success)
{
    public int Id { get; } = id;
    public string UsuarioId { get; } = usuarioId?.ToString() ?? "Sin usuario";
    public string Fecha { get; } = fecha is { } value
        ? DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
        : "Sin fecha";
    public string Resultado { get; } = success switch { true => "Correcto", false => "Fallido", null => "Sin resultado" };
}