using BackCoding.Challenge.Application.Commands.Clients;
using FluentValidation;

namespace BackCoding.Challenge.Application.Validators.Clients
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\d{10,12}$")
                .WithMessage("El número de teléfono debe tener entre 10 y 12 dígitos");
        }
    }
}
