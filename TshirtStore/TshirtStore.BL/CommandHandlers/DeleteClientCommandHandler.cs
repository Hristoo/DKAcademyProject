using AutoMapper;
using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MsSql;

namespace TshirtStore.BL.CommandHandlers
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, ClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public DeleteClientCommandHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientResponse> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var isClientExist = await _clientRepository.GetClientByName(request.clientRequest.Name);

            if (isClientExist == null)
            {
                return new ClientResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Client doesn't exist!",
                };
            }

            var client = _mapper.Map<Client>(isClientExist);    

            await _clientRepository.DeleteClient(client.Id);

            return new ClientResponse()
            {
                Client = client,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
