using BackCoding.Challenge.Application.DTOs.Subscriptions;
using FluentValidation;

namespace BackCoding.Challenge.Application.Validators.Subscriptions
{
    public class SubscribeProductValidator : AbstractValidator<SubscriptionRequest>
    {
        public SubscribeProductValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
