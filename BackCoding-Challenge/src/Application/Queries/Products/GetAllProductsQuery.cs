using BackCoding.Challenge.Application.DTOs.Products;
using MediatR;

namespace BackCoding.Challenge.Application.Queries.Products
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductResponseDto>>;
}
