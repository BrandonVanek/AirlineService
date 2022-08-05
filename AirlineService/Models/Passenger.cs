﻿namespace AirlineService.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Booking> Flights { get; set; }
    }
}
