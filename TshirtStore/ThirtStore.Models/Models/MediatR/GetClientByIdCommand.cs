using MediatR;

namespace ThirtStore.Models.Models.MediatR
{
    public record GetClientByIdCommand(int clientId) : IRequest<Client>
    {
    }
}
