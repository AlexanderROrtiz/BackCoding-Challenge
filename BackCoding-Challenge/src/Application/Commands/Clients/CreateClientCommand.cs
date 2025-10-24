using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;

namespace BackCoding.Challenge.Application.Commands.Clients
{
    public record CreateClientCommand(string FirstName, string LastName, string City, string PhoneNumber)
        : IRequest<ClientResponseDto>;

    public class CreateClientHandler : IRequestHandler<CreateClientCommand, ClientResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientResponseDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Client>();

            var client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                Balance = 500000M
            };

            await repo.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            return new ClientResponseDto
            {
                Id = client.Id,
                FullName = $"{client.FirstName} {client.LastName}",
                City = client.City,
                Balance = client.Balance,
                PhoneNumber = client.PhoneNumber
            };
        }
    }
}
