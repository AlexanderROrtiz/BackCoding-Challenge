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
    public class SubscribeProductHandlerTests
    {
        private readonly BackCodingDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private readonly Mock<INotificationStrategyResolver> _notificationResolverMock;
        private readonly Mock<INotificationService> _notificationServiceMock;

        public SubscribeProductHandlerTests()
        {
            _context = TestDbContextFactory.CreateContext();
            _unitOfWork = new UnitOfWork(_context);

            _notificationServiceMock = new Mock<INotificationService>();
            _notificationResolverMock = new Mock<INotificationStrategyResolver>();
            _notificationResolverMock.Setup(r => r.Resolve(It.IsAny<string>()))
                .Returns(_notificationServiceMock.Object);
        }

        [Fact]
        public async Task Should_Subscribe_Client_When_Valid_Data()
        {
            // Arrange
            var client = new Client { FirstName = "Roberth", LastName = "Ortiz", City = "Bogotá", Balance = 500000 };
            var product = new Product { Name = "Fondo Plus", MinAmount = 100000 };

            await _context.Clients.AddAsync(client);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var handler = new SubscribeProductHandler(_unitOfWork, _notificationResolverMock.Object, mapper: null);
            var command = new SubscribeProductCommand(new SubscriptionRequest
            {
                ClientId = client.Id,
                ProductId = product.Id,
                Amount = 150000
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.NewBalance.Should().Be(350000);
            _notificationServiceMock.Verify(n => n.SendNotificationAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Client_Has_Insufficient_Balance()
        {
            // Arrange
            var client = new Client { FirstName = "John", LastName = "Doe", City = "Medellín", Balance = 50000 };
            var product = new Product { Name = "Fondo Premium", MinAmount = 100000 };

            await _context.Clients.AddAsync(client);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var handler = new SubscribeProductHandler(_unitOfWork, _notificationResolverMock.Object, mapper: null);
            var command = new SubscribeProductCommand(new SubscriptionRequest
            {
                ClientId = client.Id,
                ProductId = product.Id,
                Amount = 120000
            });

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*saldo disponible*");
        }
    }
}
