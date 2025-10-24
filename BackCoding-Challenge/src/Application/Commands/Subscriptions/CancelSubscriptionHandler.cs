using BackCoding.Challenge.Application.Constants;
using BackCoding.Challenge.Application.DTOs.Subscriptions;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BackCoding.Challenge.Application.Commands.Subscriptions
{
    public class CancelSubscriptionHandler : IRequestHandler<CancelSubscriptionCommand, SubscriptionResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        private readonly ILogger _logger;

        public CancelSubscriptionHandler(IUnitOfWork unitOfWork, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _logger = Log.ForContext<CancelSubscriptionHandler>();
        }

        public async Task<SubscriptionResponseDto> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
        {
            var req = command.Request;

            _logger.Information("Iniciando cancelación del cliente {ClientId} para el producto {ProductId}", req.ClientId, req.ProductId);

            var clientRepo = _unitOfWork.Repository<Client>();
            var subRepo = _unitOfWork.Repository<Subscription>();
            var productRepo = _unitOfWork.Repository<Product>();
            var transRepo = _unitOfWork.Repository<Transaction>();

            var client = await clientRepo.GetByIdAsync(req.ClientId)
                ?? throw new KeyNotFoundException(MessageConstants.ClientNotFound);

            var subscription = await subRepo.GetQuery()
                .FirstOrDefaultAsync(s => s.ClientId == req.ClientId && s.ProductId == req.ProductId, cancellationToken)
                ?? throw new InvalidOperationException("No se encontró una suscripción activa para este producto.");

            var product = await productRepo.GetByIdAsync(req.ProductId)
                ?? throw new KeyNotFoundException(MessageConstants.ProductNotFound);

            client.Balance += product.MinAmount;
            clientRepo.Update(client);
            subRepo.Remove(subscription);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                ProductId = product.Id,
                Amount = product.MinAmount,
                Type = "Cancelación",
                Date = DateTime.UtcNow
            };

            await transRepo.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            await _smsService.SendSmsAsync(client.PhoneNumber, $"Cancelación exitosa del fondo {product.Name}. Nuevo saldo: {client.Balance:C}");

            _logger.Information("Cancelación completada: Cliente {ClientId}, Producto {ProductId}", client.Id, product.Id);

            return new SubscriptionResponseDto
            {
                ClientId = client.Id,
                ProductId = product.Id,
                TransactionId = transaction.Id,
                NewBalance = client.Balance,
                Message = string.Format(MessageConstants.SubscriptionCancel, product.Name)
            };
        }
    }
}
