﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Dtos
{
    public class FlightDetailUpdateDto
    {
        [Required]
        public string FlightCode { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
