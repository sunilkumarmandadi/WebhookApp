using AirlineWeb.Dtos;
using AutoMapper;
using AirlineWeb.Models;

namespace AirlineWeb.Profiles
{
    public class WebhookSubscriptionProfile : Profile
    {
        public WebhookSubscriptionProfile() 
        {
            //Source --> Target
            CreateMap<WebhookSubscriptionCreateDto, WebhookSubscription>();
            CreateMap<WebhookSubscription, WebhookSubscriptionReadDto>();
        }
    }
}
