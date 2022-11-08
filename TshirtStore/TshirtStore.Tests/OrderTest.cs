using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Requests;
using TshirtStore.AutoMapper;
using TshirtStore.BL.CommandHandlers;
using TshirtStore.Controllers;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.Tests
{
    public class OrderTest
    {
        private IList<Order> _orders = new List<Order>()
        {
            new Order()
            {
                Id = 1,
                ClientId = 2,
                LastUpdated = DateTime.UtcNow,
                Sum = 250
            },
             new Order()
            {
                Id = 2,
                ClientId = 3,
                LastUpdated = DateTime.UtcNow,
                Sum = 300
            }
        };

        private readonly IMapper _mapper;
        private Mock<ILogger<AddOrderCommandHandler>> _loggerMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderRepository _orderRepository;
        private readonly Mock<ILogger<OrderController>> _loggerOrderControllerMock;

        public OrderTest()
        {
            var mockMapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMappings()));

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<AddOrderCommandHandler>>();
            _loggerOrderControllerMock = new Mock<ILogger<OrderController>>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task Order_Add_Order_Ok()
        {
            //setup
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 255
            };

            _orderRepositoryMock.Setup(x => x.AddOrder(It.IsAny<Order>())).Callback(() =>
            {
                _orders.Add(new Order()
                {
                    Id = 3,
                    ClientId = orderRequest.ClientId,
                    LastUpdated = orderRequest.LastUpdated,
                    Sum = orderRequest.Sum
                });
            }).ReturnsAsync(() => _orders.FirstOrDefault(x => x.Id == 3));

            //inject
            var mediator = new Mock<IMediator>();

            var command = new AddOrderCommand(orderRequest);
            //var handler = new AddOrderCommandHandler(, _mapper);

            //act
            var token = new CancellationToken();
            //var x = await handler.Handle(command, token);

            //Assert
            //Assert.NotNull(x.Message);

            //var resultValue = okObjectResult.Value as AddAuthorResponse;
            //Assert.NotNull(resultValue);
            //Assert.Equal(3, resultValue.Author.Id);

        }
    }
}