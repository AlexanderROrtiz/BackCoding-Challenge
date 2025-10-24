using AutoMapper;
using BackCoding.Challenge.Application.DTOs.Subscriptions;
using BackCoding.Challenge.Domain.Entities;

namespace BackCoding.Challenge.Application.MappingProfiles
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Transaction, SubscriptionResponseDto>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NewBalance, opt => opt.Ignore());
        }
    }
}
