using AutoMapper;
using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.CommandHandlers
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientResponse> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var isClientExist = await _clientRepository.GetClientByName(request.clientRequest.Name);

            if (isClientExist == null)
            {
                return new ClientResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Client doesn't exist!"
                };
            }

            var client = _mapper.Map<Client>(request.clientRequest);
            client.Id = isClientExist.Id;
            var result = await _clientRepository.UpdateClient(client);

            return new ClientResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Message = "Client is updated successfully!",
                Client = result
            };
        }
    }
}
