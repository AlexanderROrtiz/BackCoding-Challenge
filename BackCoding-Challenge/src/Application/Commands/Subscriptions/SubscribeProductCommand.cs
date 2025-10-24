using BackCoding.Challenge.Application.DTOs.Subscriptions;
using MediatR;

namespace BackCoding.Challenge.Application.Commands.Subscriptions
{
    public record SubscribeProductCommand(SubscriptionRequest Request) : IRequest<SubscriptionResponseDto>;
}
