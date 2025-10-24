
using BackCoding.Challenge.Application.DTOs.Products;
using FluentValidation;

namespace BackCoding.Challenge.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(200);
            RuleFor(p => p.ProductType).NotEmpty().MaximumLength(100);
            RuleFor(p => p.MinAmount).GreaterThan(0);
        }
    }
}
