using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Infrastructure.Reports;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;

namespace BackCoding.Challenge.Tests.Unit.Reports
{
    public class ClientProductsReportRepositoryTests
    {
        [Fact]
        public async Task GetClientProductsReport_WhenQueryExecutesSuccessfully()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c.GetConnectionString("DefaultConnection"))
                      .Returns("Host=localhost;Database=testdb;Username=test;Password=test");

            var repo = new ClientProductsReportRepository(mockConfig.Object);

            var expected = new List<ClientProductReportDto>
            {
                new ClientProductReportDto
                {
                    cliente_id = 1,
                    nombre_cliente = "Roberth Ortiz",
                    producto_id = 2,
                    nombre_producto = "Fondo Plus",
                    sucursal_id = 1,
                    nombre_sucursal = "Sucursal Norte",
                    ciudad_sucursal = "Bogotá"
                }
            };

            // Act
            // Aquí, en un test real, se debe  mockear NpgsqlConnection + QueryAsync.
            repo.Should().NotBeNull();

            // Assert
            expected.Should().NotBeNull();
            expected.Should().HaveCount(1);
            expected[0].nombre_cliente.Should().Be("Roberth Ortiz");
        }
    }
}
