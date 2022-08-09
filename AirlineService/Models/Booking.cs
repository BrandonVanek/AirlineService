using AirlineService.DTO;

namespace AirlineService.Models
{
    public class Booking
    {
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public Flight Flight { get; set; }
        public Passenger Passenger { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
