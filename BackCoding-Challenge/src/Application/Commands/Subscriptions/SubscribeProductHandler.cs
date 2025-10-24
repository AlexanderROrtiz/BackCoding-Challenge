using AutoMapper;
using BackCoding.Challenge.Application.Constants;
using BackCoding.Challenge.Application.DTOs.Subscriptions;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BackCoding.Challenge.Application.Commands.Subscriptions
{
    public class SubscribeProductHandler : IRequestHandler<SubscribeProductCommand, SubscriptionResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationStrategyResolver _notificationResolver;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SubscribeProductHandler(
            IUnitOfWork unitOfWork,
            INotificationStrategyResolver notificationResolver,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notificationResolver = notificationResolver;
            _mapper = mapper;
            _logger = Log.ForContext<SubscribeProductHandler>();
        }

        public async Task<SubscriptionResponseDto> Handle(SubscribeProductCommand command, CancellationToken cancellationToken)
        {
            var req = command.Request;

            _logger.Information("Iniciando suscripción del cliente {ClientId} al producto {ProductId}", req.ClientId, req.ProductId);

            var clientRepo = _unitOfWork.Repository<Client>();
            var productRepo = _unitOfWork.Repository<Product>();
            var subRepo = _unitOfWork.Repository<Subscription>();
            var transRepo = _unitOfWork.Repository<Transaction>();

            var client = await clientRepo.GetByIdAsync(req.ClientId)
                ?? throw new KeyNotFoundException(MessageConstants.ClientNotFound);

            var product = await productRepo.GetByIdAsync(req.ProductId)
                ?? throw new KeyNotFoundException(MessageConstants.ProductNotFound);

            if (req.Amount < product.MinAmount)
                throw new InvalidOperationException($"El monto debe ser mínimo de {product.MinAmount}.");

            if (client.Balance < req.Amount)
                throw new InvalidOperationException(string.Format(MessageConstants.InsufficientFunds, product.Name));

            var alreadySubscribed = await subRepo.GetQuery()
                .AnyAsync(s => s.ClientId == client.Id && s.ProductId == product.Id, cancellationToken);

            if (alreadySubscribed)
                throw new InvalidOperationException(string.Format(MessageConstants.AlreadySubscribed, product.Name));

            client.Balance -= req.Amount;
            clientRepo.Update(client);

            await subRepo.AddAsync(new Subscription
            {
                ClientId = client.Id,
                ProductId = product.Id
            });

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                ProductId = product.Id,
                Amount = req.Amount,
                Type = "Apertura",
                Date = DateTime.UtcNow
            };

            await transRepo.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            var notification = _notificationResolver.Resolve("email");
            await notification.SendNotificationAsync(client.PhoneNumber, "Suscripción exitosa", $"Te has suscrito al fondo {product.Name}.");

            _logger.Information("Suscripción completada correctamente para el cliente {ClientId} en el producto {ProductId}", client.Id, product.Id);

            return new SubscriptionResponseDto
            {
                ClientId = client.Id,
                ProductId = product.Id,
                TransactionId = transaction.Id,
                NewBalance = client.Balance,
                Message = string.Format(MessageConstants.SubscriptionSuccess, product.Name)
            };
        }
    }
}
