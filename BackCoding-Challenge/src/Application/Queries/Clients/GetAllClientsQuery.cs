using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackCoding.Challenge.Application.Queries.Clients
{
    public record GetAllClientsQuery() : IRequest<IEnumerable<ClientResponseDto>>;

    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllClientsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClientResponseDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Client>();
            var clients = await repo.GetQuery().ToListAsync(cancellationToken);

            return clients.Select(c => new ClientResponseDto
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName}",
                City = c.City,
                Balance = c.Balance,
                PhoneNumber = c.PhoneNumber
            });
        }
    }
}
