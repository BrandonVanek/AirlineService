using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineService.Data;
using AirlineService.Models;
using AirlineService.DTO;

namespace AirlineService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AirlineServiceDbContext _context;

        public BookingsController(AirlineServiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int idFlight, int idPassenger)
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            var booking = await _context.Bookings.FindAsync(idFlight, idPassenger);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int idFlight, int idPassenger, Booking booking)
        {
            if (idFlight != booking.FlightId && idPassenger != booking.PassengerId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(idFlight, idPassenger))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDto)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'AirlineServiceDbContext.Booking'  is null.");
            }
            try {
                var Flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == bookingDto.FlightId);
                var Passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == bookingDto.PassengerId);

                if (Flight == null || Passenger == null)
                {
                    return Problem("Flight or Passenger does not exist.");
                }

                var booking = new Booking()
                {
                    FlightId = bookingDto.FlightId,
                    PassengerId = bookingDto.PassengerId,
                    Flight = Flight,
                    Passenger = Passenger,
                    ConfirmationNumber = bookingDto.ConfirmationNumber
                };

                if (Flight.Bookings == null)
                {
                    Flight.Bookings = new List<Booking>();
                }
                if (Passenger.Bookings == null)
                {
                    Passenger.Bookings = new List<Booking>();
                }

                _context.Bookings.Add(booking);

                Passenger.Bookings.Add(booking);

                Flight.Bookings.Add(booking);

                _context.Update(Flight);
                _context.Update(Passenger);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (BookingExists(booking.FlightId, booking.PassengerId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
                return CreatedAtAction("GetBooking", new { id = booking.FlightId + booking.PassengerId }, booking);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // DELETE: api/Bookings/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(int idFlight, int idPassenger)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(idFlight, idPassenger);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int idFlight, int idPassenger)
        {
            return (_context.Bookings?.Any(e => e.FlightId == idFlight && e.PassengerId == idPassenger)).GetValueOrDefault();
        }
    }
}
