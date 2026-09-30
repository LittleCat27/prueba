using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace avaloniaprueba.Services;

internal static class AuthErrors
{
    public static string GetMessage(Exception exception) => exception switch
    {
        TaskCanceledException => "El servidor tardó demasiado en responder. Intentá nuevamente.",
        JsonException => "El servidor devolvió una respuesta inválida. Intentá nuevamente.",
        HttpRequestException { StatusCode: HttpStatusCode.Unauthorized } => "Usuario o contraseña incorrectos.",
        HttpRequestException { StatusCode: HttpStatusCode.Conflict } => "El usuario o el correo electrónico ya están registrados.",
        HttpRequestException { StatusCode: HttpStatusCode.BadRequest } => "Los datos ingresados no son válidos. Revisá el usuario, el correo y la contraseña.",
        HttpRequestException { StatusCode: null } => "No se pudo conectar con la API. Verificá que el servidor local esté iniciado.",
        _ => "No se pudo completar la solicitud. Intentá nuevamente más tarde."
    };
}
