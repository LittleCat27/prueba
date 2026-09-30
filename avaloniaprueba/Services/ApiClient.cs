using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace avaloniaprueba.Services;

public sealed class ApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        var address = Environment.GetEnvironmentVariable("AVALONIAPRUEBA_API_URL") ?? "http://localhost:5197/";
        _httpClient = new HttpClient { BaseAddress = new Uri(address.TrimEnd('/') + "/", UriKind.Absolute), Timeout = TimeSpan.FromSeconds(15) };
    }

    public async Task<AuthResponse> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            new AuthRequest { Username = username, Password = password },
            JsonOptions,
            cancellationToken);

        return await ReadResponseAsync<AuthResponse>(response, cancellationToken);
    }

    public async Task<AuthResponse> RegisterAsync(string username, string mail, string password, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/auth/register",
            new RegisterRequest { Username = username, Mail = mail, Password = password },
            JsonOptions,
            cancellationToken);

        return await ReadResponseAsync<AuthResponse>(response, cancellationToken);
    }

    public async Task<LoginLogPage> GetLoginLogsAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"api/logs?page={page}&pageSize={pageSize}",
            cancellationToken);

        return await ReadResponseAsync<LoginLogPage>(response, cancellationToken);
    }

    private static async Task<T> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(GetErrorMessage(body, response), null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new HttpRequestException("La API devolvió una respuesta vacía.");
    }

    private static string GetErrorMessage(string body, HttpResponseMessage response)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;

            if (root.TryGetProperty("message", out var message))
                return message.GetString() ?? "La solicitud no pudo completarse.";


            if (root.TryGetProperty("errors", out var errors))
            {
                var details = errors.EnumerateObject()
                    .SelectMany(error => error.Value.EnumerateArray().Select(item => item.GetString()))
                    .Where(detail => !string.IsNullOrWhiteSpace(detail));
                var errorText = string.Join(" ", details);
                if (!string.IsNullOrWhiteSpace(errorText))
                    return errorText;
            }
        }
        catch (JsonException)
        {
        }

        return $"La API respondió con el estado {(int)response.StatusCode} ({response.ReasonPhrase}).";
    }
}

public class AuthRequest
{
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
}

public sealed class RegisterRequest : AuthRequest
{
    public string Mail { get; init; } = "";
}

public sealed class AuthResponse
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string Mail { get; init; } = "";
}

public sealed class LoginLogPage
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
    public List<LoginLogItem> Items { get; init; } = [];
}

public sealed class LoginLogItem
{
    public int Id { get; init; }
    public int? UsuarioId { get; init; }
    public DateTime? Fecha { get; init; }
    public bool? Success { get; init; }
}