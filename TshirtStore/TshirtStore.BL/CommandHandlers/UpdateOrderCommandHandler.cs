using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Responses;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MsSql;

namespace TshirtStore.BL.CommandHandlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrderByClientId(request.orderRequest.ClientId);

            if (orders == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Order doesn't exist!"
                };
            }

            var LastOrder = orders.LastOrDefault();
            var result = await _orderRepository.UpdateOrder(LastOrder);

            return new OrderResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Message = "Order is updated successfully!",
                Order = result
            };
        }
    }
}
