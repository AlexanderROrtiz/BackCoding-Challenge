using BackCoding.Challenge.Application.DTOs.Subscriptions;
using MediatR;

namespace BackCoding.Challenge.Application.Commands.Subscriptions
{
    public record CancelSubscriptionCommand(SubscriptionRequest Request) : IRequest<SubscriptionResponseDto>;
   
}
