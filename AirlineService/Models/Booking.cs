using AirlineService.DTO;
using System.Text.Json.Serialization;

namespace AirlineService.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        [JsonIgnore]
        public virtual Flight Flight { get; set; }
        [JsonIgnore]
        public virtual Passenger Passenger { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
