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
        private readonly IMapper _mapper;

        public AddOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.orderRequest);
            var result = _orderRepository.AddOrder(order);

            if (result == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Something went wrong! Order wasn't added!"
                };
            }

            return new OrderResponse ()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Order = order
            };
        }
    }
}
