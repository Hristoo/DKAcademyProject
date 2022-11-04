using MediatR;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.CommandHandlers
{
    public class GetOrderByIdCommandHandler : IRequestHandler<GetOrderByIdCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

         public async Task<Order> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(request.id);

            return order;
        }
    }
}
