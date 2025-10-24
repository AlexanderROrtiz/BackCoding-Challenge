using BackCoding.Challenge.WebApi.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace BackCoding.Challenge.Tests.Unit.Middlewares
{
    public class ExceptionMiddlewareTests
    {
        [Theory]
        [InlineData(typeof(KeyNotFoundException), HttpStatusCode.NotFound)]
        [InlineData(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
        [InlineData(typeof(AuthenticationException), HttpStatusCode.Unauthorized)]
        [InlineData(typeof(SecurityTokenException), HttpStatusCode.Unauthorized)]
        [InlineData(typeof(UnauthorizedAccessException), HttpStatusCode.Forbidden)]
        [InlineData(typeof(Exception), HttpStatusCode.InternalServerError)]
        public async Task ReturnExpectedStatusCode(Type exceptionType, HttpStatusCode expectedStatus)
        {
            // Arrange
            var middleware = new ExceptionMiddleware(_ => throw (Exception)Activator.CreateInstance(exceptionType)!);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)expectedStatus);

            // Validamos que el cuerpo contenga el mensaje y el status
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var json = JsonDocument.Parse(responseText);

            json.RootElement.GetProperty("status").GetInt32()
                .Should().Be((int)expectedStatus);

            json.RootElement.TryGetProperty("error", out _).Should().BeTrue();
            json.RootElement.TryGetProperty("traceId", out _).Should().BeTrue();
            json.RootElement.TryGetProperty("timestamp", out _).Should().BeTrue();
        }

        [Fact]
        public async Task NotThrow_When_NoException()
        {
            // Arrange
            var mockNext = new Mock<RequestDelegate>();
            mockNext.Setup(n => n(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);
            var middleware = new ExceptionMiddleware(mockNext.Object);
            var context = new DefaultHttpContext();

            // Act
            Func<Task> act = async () => await middleware.InvokeAsync(context);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}
