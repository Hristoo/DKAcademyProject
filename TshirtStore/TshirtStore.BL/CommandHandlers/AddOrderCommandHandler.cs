using AutoMapper;
using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.CommandHandlers
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITshirtRepository _shirtRepository;
        private readonly IMapper _mapper;

        public AddOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IClientRepository clientRepository, ITshirtRepository shirtRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _shirtRepository = shirtRepository;
        }

        public async Task<OrderResponse> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.orderRequest);
            var isClientExist = await _clientRepository.GetById(order.ClientId);
            var tshirts = order.Tshirts;

            foreach (var orderTshirt in tshirts)
            {
                var tshirt = await _shirtRepository.GetTshirtsById(orderTshirt.Id);

                if (tshirt == null)
                {
                    return new OrderResponse()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = $"No tshirt added! Add tshirt and try again!"
                    };
                }

                if (tshirt.Quantity < orderTshirt.Quantity)
                {
                    return new OrderResponse()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = $"No enough quantity {tshirt.Name} tshirts! Try again later!"
                    };
                }

                tshirt.Quantity -= orderTshirt.Quantity;

                await _shirtRepository.UpdateThirt(tshirt);
            }

            var result = _orderRepository.AddOrder(order);

            if (result == null || isClientExist == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Something went wrong! Order wasn't added!"
                };
            }

            return new OrderResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Message = "Order was added successfully!",
                Order = order
            };
        }
    }
}
