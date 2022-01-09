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
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _airlineDbContext;
        private readonly IMapper _mapper;

        public WebhookSubscriptionController(AirlineDbContext airlineDbContext, IMapper mapper)
        {
            _airlineDbContext = airlineDbContext;
            _mapper = mapper;
        }
        [HttpGet("{secret}", Name = "GetSubscriptionBySecret")]
        public ActionResult<WebhookSubscriptionReadDto> GetSubscriptionBySecret(string secret)
        {
            var subscription = _airlineDbContext.WebhookSubscriptions.FirstOrDefault(x => x.Secret == secret);
            if (subscription == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
            }
        }

        [HttpPost]
        public ActionResult<WebhookSubscriptionReadDto> CreateSubscription(WebhookSubscriptionCreateDto webhookSubscriptionCreateDto)
        {
            var subscription = _airlineDbContext.WebhookSubscriptions.FirstOrDefault(s => s.WebhookURI == webhookSubscriptionCreateDto.WebhookURI);
            if (subscription == null)
            {
                subscription = _mapper.Map<WebhookSubscription>(webhookSubscriptionCreateDto);
                subscription.Secret = Guid.NewGuid().ToString();
                subscription.WebhookPublisher = "PanAme";
                try
                {
                    _airlineDbContext.WebhookSubscriptions.Add(subscription);
                    _airlineDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                var webhookSubscriptionReadDto = _mapper.Map<WebhookSubscriptionReadDto>(subscription);
                return CreatedAtRoute(nameof(GetSubscriptionBySecret), new { secret = webhookSubscriptionReadDto.Secret }, webhookSubscriptionReadDto);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
