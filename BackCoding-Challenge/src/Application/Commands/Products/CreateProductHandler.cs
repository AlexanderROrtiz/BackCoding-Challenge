
using AutoMapper;
using BackCoding.Challenge.Application.DTOs.Products;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Serilog;

namespace BackCoding.Challenge.Application.Commands.Products
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = Log.ForContext<CreateProductHandler>();
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            _logger.Information("Creando producto {@Request}", command.Request);

            var repo = _unitOfWork.Repository<Product>();

            var entity = _mapper.Map<Product>(command.Request);
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.Information("Producto creado correctamente con Id {Id}", entity.Id);

            return _mapper.Map<ProductResponseDto>(entity);
        }
    }
}
