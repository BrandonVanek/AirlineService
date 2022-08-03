using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineService.Data;
using AirlineService.Models;

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
          if (_context.Booking == null)
          {
              return NotFound();
          }
            return await _context.Booking.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
          if (_context.Booking == null)
          {
              return NotFound();
          }
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.FlightId)
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
                if (!BookingExists(id))
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
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
          if (_context.Booking == null)
          {
              return Problem("Entity set 'AirlineServiceDbContext.Booking'  is null.");
          }
            _context.Booking.Add(booking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.FlightId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooking", new { id = booking.FlightId }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Booking == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Booking?.Any(e => e.FlightId == id)).GetValueOrDefault();
        }
    }
}
