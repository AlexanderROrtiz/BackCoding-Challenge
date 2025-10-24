
using AutoMapper;
using BackCoding.Challenge.Application.DTOs.Products;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Serilog;

namespace BackCoding.Challenge.Application.Queries.Products
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetProductByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = Log.ForContext<GetProductByIdHandler>();
        }

        public async Task<ProductResponseDto> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.Information("Consultando producto con Id {Id}", query.Id);

            var repo = _unitOfWork.Repository<Product>();
            var product = await repo.GetByIdAsync(query.Id);

            if (product == null)
            {
                _logger.Warning("Producto con Id {Id} no encontrado", query.Id);
                return null!;
            }

            var result = _mapper.Map<ProductResponseDto>(product);
            _logger.Information("Producto encontrado {@Product}", result);
            return result;
        }
    }
}
