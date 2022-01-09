﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Dtos
{
    public class NotificationMessageDto
    {
        public NotificationMessageDto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
        public string WebhookType { get; set; }
        public string FlightCode { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
    }
}
