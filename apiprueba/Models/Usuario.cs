using System;
using System.Collections.Generic;

namespace apiprueba.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Mail { get; set; }

    public bool? IsActive { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<LoginLog> LoginLogs { get; set; } = new List<LoginLog>();
}
