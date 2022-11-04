using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MsSql;

namespace TshirtStore.BL.CommandHandlers
{
    public class GetClientByIdCommandHandler : IRequestHandler<GetClientByIdCommand, Client>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientByIdCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }


        public async Task<Client> Handle(GetClientByIdCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetById(request.clientId);

            return client;
        }
    }
}
