using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class Vehicle
{
    public string VehicleId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<CustomerVehicle> CustomerVehicles { get; set; } = new List<CustomerVehicle>();
}
