using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Session
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Ip { get; set; }

    public bool IsValid { get; set; }

    public virtual User User { get; set; } = null!;
}
