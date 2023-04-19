using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<CustomerVehicle> CustomerVehicles { get; set; } = new List<CustomerVehicle>();

    public virtual ICollection<HeaderTrx> HeaderTrxes { get; set; } = new List<HeaderTrx>();
}
