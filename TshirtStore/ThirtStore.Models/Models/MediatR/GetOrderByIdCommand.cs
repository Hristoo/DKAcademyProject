using MediatR;

namespace ThirtStore.Models.Models.MediatR
{
    public record GetOrderByIdCommand(int id) : IRequest<Order>
    {
    }
}
