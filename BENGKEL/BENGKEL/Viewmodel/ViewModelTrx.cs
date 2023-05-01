using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BENGKEL.Viewmodel
{
    public class ViewModelTrx
    {
        public string TrxId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public int CustomerVehicleId { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}
