using apiprueba.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiprueba.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var username = request.Username.Trim().ToLower();

        var mail = request.Mail.Trim().ToLower();

        if (username.Length < 3)
            return BadRequest(new { message = "El usuario debe tener al menos 3 caracteres." });

        
        await using var transaction = await db.Database.BeginTransactionAsync();

        if (await db.Usuarios.AnyAsync(u => u.Username != null && u.Username.ToLower() == username
            || u.Mail != null && u.Mail.ToLower() == mail))
            return Conflict(new { message = "El usuario o el mail ya están registrados." });


        var user = new Usuario
        {
            Username = username, Mail = mail, IsActive = true
        };

        user.Password = HashPassword(request.Password);

        db.Usuarios.Add(user);

        await db.SaveChangesAsync();
        await transaction.CommitAsync();


        return StatusCode(201, new { user.Id, user.Username, user.Mail }); //201 Created
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var username = request.Username.Trim().ToLower();

        var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Username != null && u.Username.ToLower() == username);



            //Colocar un ? delante de user para que no tire error si es null, y que devuelva false en ese caso.
        var success = user?.IsActive == true && user.Password == HashPassword(request.Password);
        
        db.LoginLogs.Add(new LoginLog
        {
            UsuarioId = user?.Id, Fecha = DateTime.UtcNow, Success = success
        });
        await db.SaveChangesAsync();
        if (!success)
            return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
        return Ok(new { user!.Id, user.Username, user.Mail });
    }

    // Hay mejores opciones para las contraseñas que SHA-256.
    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
