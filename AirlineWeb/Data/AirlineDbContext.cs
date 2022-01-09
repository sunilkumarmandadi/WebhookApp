﻿using AirlineWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineWeb.Data
{
    public class AirlineDbContext: DbContext
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> opt) : base(opt)
        {

        }

        public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
        public DbSet<FlightDetail> FlightDetails { get; set; }
    }
}
