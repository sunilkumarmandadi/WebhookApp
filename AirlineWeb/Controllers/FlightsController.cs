﻿using AirlineWeb.Data;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
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
        private readonly IMessageBusClient _messageBusClient;

        public FlightsController(AirlineDbContext context, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _context = context;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
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
                decimal oldPrice = flight.Price;

                _mapper.Map(flightDetailUpdateDto, flight);
                try
                {
                    _context.SaveChanges();
                    if (oldPrice != flight.Price)
                    {
                        Console.WriteLine("Price change. Place message on bus");
                        var message = new NotificationMessageDto
                        {
                            WebhookType = "pricechange",
                            OldPrice = oldPrice,
                            NewPrice = flight.Price,
                            FlightCode = flight.FlightCode
                        };
                        _messageBusClient.SendMessage(message);
                    }
                    else
                    {
                        Console.WriteLine("No price change");
                    }
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                
               
            }
        }
    }
}
