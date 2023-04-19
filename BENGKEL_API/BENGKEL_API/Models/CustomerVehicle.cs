using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class CustomerVehicle
{
    public int CustomerVehicleId { get; set; }

    public string CustomerId { get; set; } = null!;

    public string VehicleId { get; set; } = null!;

    public string Number { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DetailTrx> DetailTrxes { get; set; } = new List<DetailTrx>();

    public virtual Vehicle Vehicle { get; set; } = null!;
}
