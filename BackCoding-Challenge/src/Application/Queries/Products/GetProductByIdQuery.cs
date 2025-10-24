
using BackCoding.Challenge.Application.DTOs.Products;
using MediatR;

namespace BackCoding.Challenge.Application.Queries.Products
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductResponseDto>;
}
