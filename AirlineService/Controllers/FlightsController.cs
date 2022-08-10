﻿using System;
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
    public class FlightsController : ControllerBase
    {
        private readonly AirlineServiceDbContext _context;

        public FlightsController(AirlineServiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
          if (_context.Flights == null)
          {
              return NotFound();
          }
            return await _context.Flights
                .Include(p => p.Bookings)
                //.ThenInclude(cs => cs.Passenger)
                .ToListAsync();
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
          if (_context.Flights == null)
          {
              return NotFound();
          }
            var flight = await _context.Flights
               .Include(s => s.Bookings)
               .ThenInclude(cs => cs.Passenger)
               .FirstOrDefaultAsync(s => s.Id == id);
            //var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, FlightDTO flightDto)
        {
            //if (id != flight.Id)
            //{
            //    return BadRequest();
            //}

            if(flightDto == null)
            {
                return BadRequest();
            }

            var flight = await _context.Flights
               .FirstOrDefaultAsync(s => s.Id == id);

            if (flight == null)
            {
                return Problem("Flight does not exist.");
            }

            flight.FlightNumber = flightDto.FlightNumber;
            flight.ArrivalAirport = flightDto.ArrivalAirport;
            flight.DepartureAirport = flightDto.DepartureAirport;
            flight.ArrivalDateTime = flightDto.ArrivalDateTime;
            flight.DepartureDateTime = flightDto.DepartureDateTime;
            flight.Destination = flightDto.Destination;
            flight.MaxCapacity = flightDto.MaxCapacity;
            _context.Update(flight);


            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
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

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(FlightDTO flightDto)
        {
          if (_context.Flights == null)
          {
              return Problem("Entity set 'FlightDbContext.Flights'  is null.");
          }

            var bookings = new List<Booking>();
            var flight = new Flight()
            {
                FlightNumber = flightDto.FlightNumber,
                Destination = flightDto.Destination,
                DepartureDateTime = flightDto.DepartureDateTime,
                ArrivalDateTime = flightDto.ArrivalDateTime,
                DepartureAirport = flightDto.DepartureAirport,
                ArrivalAirport = flightDto.ArrivalAirport,
                MaxCapacity = flightDto.MaxCapacity,
                Bookings = bookings
            };

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.Id }, flightDto);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            if (_context.Flights == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights.Include(p => p.Bookings).FirstOrDefaultAsync(s => s.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            foreach (var booking in flight.Bookings)
            {
                _context.Bookings.Remove(booking);
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightExists(int id)
        {
            return (_context.Flights?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
