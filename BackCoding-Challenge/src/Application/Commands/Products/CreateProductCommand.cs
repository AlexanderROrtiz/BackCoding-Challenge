using BackCoding.Challenge.Application.DTOs.Products;
using MediatR;

namespace BackCoding.Challenge.Application.Commands.Products
{
    public record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductResponseDto>;
}
