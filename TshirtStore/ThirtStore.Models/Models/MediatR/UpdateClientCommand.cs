using MediatR;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;

namespace ThirtStore.Models.Models.MediatR
{
    public record UpdateClientCommand(ClientRequest clientRequest) : IRequest<ClientResponse>
    {
    }
}
