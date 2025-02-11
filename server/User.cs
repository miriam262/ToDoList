using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string Password { get; set; } = null!;

    // public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
