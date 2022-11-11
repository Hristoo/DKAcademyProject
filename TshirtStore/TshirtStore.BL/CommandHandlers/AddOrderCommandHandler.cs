using AutoMapper;
using Kafka.Interfaces;
using Kafka.Services;
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
        private readonly ITshirtRepository _tshirtRepository;
        private readonly IMapper _mapper;
        private readonly IGenericProducer<int, Order> _producer;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public AddOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IClientRepository clientRepository, ITshirtRepository shirtRepository, IGenericProducer<int, Order> producer, IShoppingCartRepository shoppingCartRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _tshirtRepository = shirtRepository;
            _producer = producer;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<OrderResponse> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartRepository.GetContent(request.clientId);
            var isClientExist = await _clientRepository.GetById(request.clientId);
            var order = new Order() { 
                Sum = 0,
                ClientId = cart.ClientId,
                LastUpdated = DateTime.UtcNow, 
                Tshirts = cart.Tshirts,
            };

            foreach (var orderTshirt in cart.Tshirts)
            {
                var tshirt = await _tshirtRepository.GetTshirtsById(orderTshirt.Id);

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

                await _tshirtRepository.UpdateThirt(tshirt);
                order.Sum += (tshirt.Price * orderTshirt.Quantity);
            }

            var result = await _orderRepository.AddOrder(order);

            await _producer.SendMessage(result.Id, result);

            if (result == null || isClientExist == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Something went wrong! Order wasn't added!"
                };
            }

            await _shoppingCartRepository.EmptyCart(cart.Id);

            return new OrderResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Message = "Order was added successfully!",
                Order = order
            };
        }
    }
}
