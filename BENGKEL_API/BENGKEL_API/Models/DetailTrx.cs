using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class DetailTrx
{
    public int DetailTrxId { get; set; }

    public string TrxId { get; set; } = null!;

    public int CustomerVehicleId { get; set; }

    public int Cost { get; set; }

    public virtual CustomerVehicle CustomerVehicle { get; set; } = null!;

    public virtual HeaderTrx Trx { get; set; } = null!;
}
