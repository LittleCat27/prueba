using System.ComponentModel.DataAnnotations;

namespace apiprueba.Models;

public class RegisterRequest
{

    //Estos data annotations son para validar los datos de entrada en el request. 
    // Por ejemplo, si el usuario no envía un mail válido, se devuelve un error 400 Bad Request.
    [Required, StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = "";
    [Required, EmailAddress, StringLength(255)]
    public string Mail { get; set; } = "";
    [Required, StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = "";
}

public class LoginRequest
{
    [Required, StringLength(100)]
    public string Username { get; set; } = "";
    [Required, StringLength(128)]
    public string Password { get; set; } = "";
}
