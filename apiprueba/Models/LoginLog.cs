using System;
using System.Collections.Generic;

namespace apiprueba.Models;

public partial class LoginLog
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public bool? Success { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
