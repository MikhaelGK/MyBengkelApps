using System;
using System.Collections.Generic;

namespace BENGKEL_API.Models;

public partial class HeaderTrx
{
    public string TrxId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string CustomerId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DetailTrx> DetailTrxes { get; set; } = new List<DetailTrx>();

    public virtual Employee Employee { get; set; } = null!;
}
