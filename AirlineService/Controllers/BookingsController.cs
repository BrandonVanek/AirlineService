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
            return await _context.Bookings
                //.Include(p => p.Flight)
                //.Include(p => p.Passenger)
                .ToListAsync();
        }

        //// GET: api/Bookings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Booking>> GetBooking(int idFlight, int idPassenger)
        //{
        //  if (_context.Bookings == null)
        //  {
        //      return NotFound();
        //  }
        //    var booking = await _context.Bookings
        //        .Include(p => p.Flight)
        //        .Include(p => p.Passenger)
        //        .FirstOrDefaultAsync(p => p.FlightId == idFlight && p.PassengerId == idPassenger);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    return booking;
        //}

        /*
         * REMOVED: WOULD DISTURB DATABASE KEYS
         */
        //// PUT: api/Bookings/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBooking(int idFlight, int idPassenger, BookingDTO bookingDto)
        //{
        //    //if (idFlight != bookingDto.FlightId && idPassenger != bookingDto.PassengerId)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    var booking = await _context.Bookings
        //        .Include(p => p.Flight)
        //        .Include(p => p.Passenger)
        //        .FirstOrDefaultAsync(s => s.FlightId == idFlight && s.PassengerId == idPassenger);

        //    if (booking == null)
        //    {
        //        return Problem("This ticket does not exist.");
        //    }

        //    var Flight = await _context.Flights
        //        .Include(p => p.Bookings)
        //        .FirstOrDefaultAsync(f => f.Id == bookingDto.FlightId);
        //    var Passenger = await _context.Passengers
        //        .Include(p => p.Bookings)
        //        .FirstOrDefaultAsync(p => p.Id == bookingDto.PassengerId);

        //    if (Flight == null || Passenger == null)
        //    {
        //        return Problem("Flight or Passenger does not exist.");
        //    }
        //    if (Flight.MaxCapacity == _context.Bookings.Count())
        //    {
        //        return Problem("Flight has reached its maximum capacity.");
        //    }

        //    //if (bookingDto.FlightId != idFlight || bookingDto.PassengerId != idPassenger)
        //    //{
        //    //    Passenger.Bookings.Remove(bookingTmp);
        //    //    Flight.Bookings.Remove(bookingTmp);
        //    //    _context.Bookings.Remove(bookingTmp);
        //    //}

        //    booking.FlightId = bookingDto.FlightId;
        //    booking.PassengerId = bookingDto.PassengerId;
        //    booking.Flight = Flight;
        //    booking.Passenger = Passenger;
        //    booking.ConfirmationNumber = bookingDto.ConfirmationNumber;
        //    _context.Update(booking);

        //    //_context.Update(booking.Passenger);

        //    _context.Entry(booking).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BookingExists(idFlight, idPassenger))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDto)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'AirlineServiceDbContext.Booking'  is null.");
            }
            var Flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == bookingDto.FlightId);
            var Passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == bookingDto.PassengerId);

            if (Flight == null || Passenger == null)
            {
                return Problem("Flight or Passenger does not exist.");
            }

            //if (Flight.MaxCapacity == _context.Bookings.Count())
            //{
            //    return Problem("Flight has reached its maximum capacity.");
            //}

            var booking = new Booking()
            {
                FlightId = bookingDto.FlightId,
                PassengerId = bookingDto.PassengerId,
                ConfirmationNumber = bookingDto.ConfirmationNumber,
                Flight = Flight,
                Passenger = Passenger,
            };
            _context.Bookings.Add(booking);
            Passenger.Bookings.Add(booking);
            Flight.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
            
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Flight != null)
            {
                booking.Flight.Bookings.Remove(booking);
            }
                
            if (booking.Passenger != null)
            {
                booking.Passenger.Bookings.Remove(booking);
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
