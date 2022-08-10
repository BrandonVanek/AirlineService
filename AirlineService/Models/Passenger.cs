using AirlineService.DTO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineService.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        //[ForeignKey("PassengerId")]
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
