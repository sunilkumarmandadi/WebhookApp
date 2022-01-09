using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;

        public FlightsController(AirlineDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("{flightCode}", Name = "GetFlightDetailsByCode")]
        public ActionResult<FlightDetailReadDto> GetFlightDetailsByCode(string flightCode)
        {
            var flight = _context.FlightDetails.FirstOrDefault(x => x.FlightCode == flightCode);
            if (flight == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<FlightDetailReadDto>(flight));
            }
        }
        [HttpPost]
        public ActionResult<FlightDetailReadDto> CreateFlight(FlightDetailCreateDto flightDetailCreateDto)
        {
            var flight = _context.FlightDetails.FirstOrDefault(x => x.FlightCode == flightDetailCreateDto.FlightCode);
            if (flight == null)
            {
                var flightDetailModel = _mapper.Map<FlightDetail>(flightDetailCreateDto);

                try
                {
                    _context.FlightDetails.Add(flightDetailModel);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                var flightDetailReadDto = _mapper.Map<FlightDetailReadDto>(flightDetailModel);
                return CreatedAtRoute(nameof(GetFlightDetailsByCode), new { flightCode = flightDetailReadDto.FlightCode }, flightDetailReadDto);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateFlightDetails(int id, FlightDetailUpdateDto flightDetailUpdateDto)
        {
            var flight = _context.FlightDetails.FirstOrDefault(x => x.Id == id);
            if (flight == null)
            {
                return NotFound();
            }
            else
            {
                _mapper.Map(flightDetailUpdateDto, flight);
                _context.SaveChanges();
                return NoContent();
            }
        }
    }
}
