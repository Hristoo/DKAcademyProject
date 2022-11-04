using AutoMapper;
using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MsSql;

namespace TshirtStore.BL.CommandHandlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var isOrderExist = await _orderRepository.GetOrderById(request.orderId);

            if (isOrderExist == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Order doesn't exist!",
                };
            }

            var order = _mapper.Map<Order>(isOrderExist);

            await _orderRepository.DeleteOrder(order.Id);

            return new OrderResponse()
            {
                Order = order,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
