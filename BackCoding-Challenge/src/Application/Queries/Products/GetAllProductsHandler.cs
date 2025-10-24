using AutoMapper;
using BackCoding.Challenge.Application.DTOs.Products;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackCoding.Challenge.Application.Queries.Products
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product>();
            var products = await repo.GetQuery().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }
    }
}
