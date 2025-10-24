using BackCoding.Challenge.Application.Commands.Subscriptions;
using BackCoding.Challenge.Application.DTOs.Subscriptions;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using BackCoding.Challenge.Infrastructure.Persistence.Context;
using BackCoding.Challenge.Infrastructure.Repositories;
using BackCoding.Challenge.Tests.Unit.Helpers;
using FluentAssertions;
using Moq;

namespace BackCoding.Challenge.Tests.Unit.Commands
{
    public class CancelSubscriptionHandlerTests
    {
        private readonly BackCodingDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly Mock<ISmsService> _mockSmsService;

        public CancelSubscriptionHandlerTests()
        {
            _context = TestDbContextFactory.CreateContext();
            _unitOfWork = new UnitOfWork(_context);
            _mockSmsService = new Mock<ISmsService>();
        }

        [Fact]
        public async Task Should_Cancel_Subscription_And_Refund_MinAmount()
        {
            // Arrange
            var client = new Client { FirstName = "Pepito", LastName = "Pérez", City = "Bogotá", Balance = 350000, PhoneNumber = "3001234567" };
            var product = new Product { Name = "Fondo Plus", MinAmount = 100000 };
            var subscription = new Subscription { Client = client, Product = product, ClientId = client.Id, ProductId = product.Id };

            await _context.Clients.AddAsync(client);
            await _context.Products.AddAsync(product);
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            var handler = new CancelSubscriptionHandler(_unitOfWork, _mockSmsService.Object);
            var command = new CancelSubscriptionCommand(new SubscriptionRequest
            {
                ClientId = client.Id,
                ProductId = product.Id,
                Amount = 100000
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.NewBalance.Should().Be(450000);
            _context.Subscriptions.Should().BeEmpty();
            _mockSmsService.Verify(s => s.SendSmsAsync(client.PhoneNumber, It.IsAny<string>()), Times.Once);
        }
    }
}
