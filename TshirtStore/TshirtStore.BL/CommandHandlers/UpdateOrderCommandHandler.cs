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
        private readonly ITshirtRepository _tshirtRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ITshirtRepository shirtRepository)
        {
            _orderRepository = orderRepository;
            _tshirtRepository = shirtRepository;
        }

        public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(request.order.Id);

            if (order == null)
            {
                return new OrderResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Order doesn't exist!"
                };
            }

            var tshirts = request.order.Tshirts;
            request.order.Sum = 0;

            foreach (var orderTshirt in tshirts)
            {
                var tshirt = await _tshirtRepository.GetTshirtsById(orderTshirt.Id);

                if (tshirt == null)
                {
                    return new OrderResponse()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = $"{orderTshirt.Name} missing in DB!"
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
                request.order.Sum += (tshirt.Price * orderTshirt.Quantity);
            }

            var result = await _orderRepository.UpdateOrder(request.order);

            return new OrderResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Message = "Order is updated successfully!",
                Order = result
            };
        }
    }
}
