using Microsoft.Identity.Client;

namespace BENGKEL_API.Viewmodel
{
    public class CustomerVehiclesDto
    {
        public int CustomerVehicleId { get; set; }
        public string VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
    }
}
