using System.ComponentModel.DataAnnotations;
using apiprueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiprueba.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 20)
    {
        var query = db.LoginLogs;
        var total = await query.CountAsync();
        var offset = (page - 1) * pageSize;

        var items = await query.OrderByDescending(l => l.Fecha)
            .Skip(offset).Take(pageSize)
            .Select(l => new { l.Id, l.UsuarioId, l.Fecha, l.Success }).ToListAsync();
            
        return Ok(new { page, pageSize, total, items });
    }
}
