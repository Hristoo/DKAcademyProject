using System.Net;
using AutoMapper;
using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.CommandHandlers
{
    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, ClientResponse>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public AddClientCommandHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientResponse> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
         

            var client = _mapper.Map<Client>(request.clientRequest);
            var result = await _clientRepository.AddClient(client);

            return new ClientResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Client = result
            };
        }
    }
}
