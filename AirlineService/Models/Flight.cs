using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineService.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDateTime { get; set; }  
        public DateTime ArrivalDateTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public int MaxCapacity { get; set; }
        [ForeignKey("FlightId")]
        public virtual ICollection<Passenger> Passengers { get; set; }
    }
}
