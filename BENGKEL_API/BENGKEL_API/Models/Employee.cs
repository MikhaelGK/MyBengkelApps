using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<HeaderTrx> HeaderTrxes { get; set; } = new List<HeaderTrx>();
}
