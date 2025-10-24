
using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;

namespace BackCoding.Challenge.Application.Queries.Clients
{
    public record GetClientByIdQuery(int Id) : IRequest<ClientResponseDto?>;

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientResponseDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClientByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientResponseDto?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Client>();
            var client = await repo.GetByIdAsync(request.Id);

            if (client == null)
                return null;

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
