namespace BENGKEL_API.Viewmodel
{
    public class TrxesDto
    {
        public string ID { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string VehicleId { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }

    public class DetailTrxDto
    {
        public string TrxId { get; set; }
        public string Date { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}
