using AirlineService.DTO;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace AirlineService.Models
{
    public class Booking
    {
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        [JsonIgnore]
        public virtual Flight Flight { get; set; }
        [JsonIgnore]
        public virtual Passenger Passenger { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
