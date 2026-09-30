using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace avaloniaprueba.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        var address = "http://localhost:5197/";
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(address.TrimEnd('/') + "/");
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            new AuthRequest { Username = username, Password = password });

        // Si la API devuelve un error HTTP, se lanza una excepción.
        response.EnsureSuccessStatusCode();

        // Convertimos el JSON recibido en un objeto de C#.
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result == null)
        {
            throw new HttpRequestException("La API devolvió una respuesta vacía.");
        }

        return result;
    }

    public async Task<AuthResponse> RegisterAsync(string username, string mail, string password)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/register",
            new RegisterRequest { Username = username, Mail = mail, Password = password });

        // Si la API devuelve un error HTTP, se lanza una excepción.
        response.EnsureSuccessStatusCode();

        // Convertimos el JSON recibido en un objeto de C#.
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result == null)
        {
            throw new HttpRequestException("La API devolvió una respuesta vacía.");
        }

        return result;
    }

    public async Task<LoginLogPage> GetLoginLogsAsync(int page = 1, int pageSize = 20)
    {
        using var response = await _httpClient.GetAsync(
            $"api/logs?page={page}&pageSize={pageSize}");

        // Si la API devuelve un error HTTP, se lanza una excepción.
        response.EnsureSuccessStatusCode();

        // Convertimos el JSON recibido en un objeto de C#.
        var result = await response.Content.ReadFromJsonAsync<LoginLogPage>();
        if (result == null)
        {
            throw new HttpRequestException("La API devolvió una respuesta vacía.");
        }

        return result;
    }

}

// Clases que representan los datos enviados y recibidos de la API.
public class AuthRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Mail { get; set; } = "";
}

public class AuthResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Mail { get; set; } = "";
}

public class LoginLogPage
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<LoginLogItem> Items { get; set; } = new List<LoginLogItem>();
}

public class LoginLogItem
{
    public int Id { get; set; }
    public int? UsuarioId { get; set; }
    public DateTime? Fecha { get; set; }
    public bool? Success { get; set; }
}
