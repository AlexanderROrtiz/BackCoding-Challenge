using FluentValidation;

namespace BackCoding.Challenge.Application.Validators
{
    public class CreateSubscriptionDto
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionDto>
    {
        public CreateSubscriptionValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
