using AutoMapper;
using Confluent.Kafka;
using Kafka.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;
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
                 ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 255,
                Tshirts = new List<Tshirt>()
                    {
                        new Tshirt()
                        {
                            Id = 55,
                            Color = "Red",
                        },
                        new Tshirt()
                        {
                            Id = 15,
                            Color = "Red",
                        }
                    }
            }
        };

        private readonly IMapper _mapper;
        private Mock<ILogger<AddOrderCommandHandler>> _loggerMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ILogger<OrderController>> _loggerOrderControllerMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ITshirtRepository> _tshirtRepositoryMock;

        public OrderTest()
        {
            var mockMapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMappings()));

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<AddOrderCommandHandler>>();
            _loggerOrderControllerMock = new Mock<ILogger<OrderController>>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _tshirtRepositoryMock = new Mock<ITshirtRepository>();
        }

        [Fact]
        public async Task Order_Add_Order_Ok()
        {
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 255
            };
            _orderRepositoryMock.Setup(r => r.AddOrder(It.IsAny<Order>())).ReturnsAsync(() => _orders.FirstOrDefault(o => o.Id == 13));

            var handler = new AddOrderCommandHandler(_orderRepositoryMock.Object, _mapper, _clientRepositoryMock.Object, _tshirtRepositoryMock.Object);
            var result = await handler.Handle(new AddOrderCommand(orderRequest), new CancellationToken());
            var mediator = new Mock<IMediator>();

            Assert.IsType<OrderResponse>(result);
            Assert.Equal(25, result.Order.ClientId);
        }

        [Fact]
        public async Task Order_Add_Order_BadPath()
        {
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 255,
                Tshirts = new List<Tshirt>()
                    {
                        new Tshirt()
                        {
                            Id = 55,
                            Color = "Red",
                        },
                        new Tshirt()
                        {
                            Id = 15,
                            Color = "Red",
                        }
                    }
            };

            _orderRepositoryMock.Setup(r => r.AddOrder(It.IsAny<Order>())).ReturnsAsync(_orders.FirstOrDefault(o => o.Id == 13));

            var handler = new AddOrderCommandHandler(_orderRepositoryMock.Object, _mapper, _clientRepositoryMock.Object, _tshirtRepositoryMock.Object);
            var result = await handler.Handle(new AddOrderCommand(orderRequest), new CancellationToken());

            Assert.Equal("No tshirt added! Add tshirt and try again!", result.Message);
            Assert.Equal("BadRequest", result.HttpStatusCode.ToString());
            Assert.Null(result.Order);
        }

        [Fact]
        public async Task Order_Delete_Order_Ok()
        {
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 255,
                Tshirts = new List<Tshirt>()
                    {
                        new Tshirt()
                        {
                            Id = 55,
                            Color = "Red",
                        },
                        new Tshirt()
                        {
                            Id = 15,
                            Color = "Red",
                        }
                    }
            };

            var ordCount = _orders.Count();
            var order = _orders.FirstOrDefault(o => o.Id == _mapper.Map<Order>(orderRequest).Id); // order is null !!!!!!!!!!

            _orderRepositoryMock.Setup(o => o.GetOrderById(order.Id)).ReturnsAsync(_orders.FirstOrDefault(o => o.Id == order.Id));
            _orderRepositoryMock.Setup(o => o.DeleteOrder(order.Id)).Callback(() =>
            {
                _orders.Remove(order);
            }).ReturnsAsync(order);

            var handler = new AddOrderCommandHandler(_orderRepositoryMock.Object, _mapper, _clientRepositoryMock.Object, _tshirtRepositoryMock.Object);
            var result = await handler.Handle(new AddOrderCommand(orderRequest), new CancellationToken());

            Assert.Equal("Ok", result.HttpStatusCode.ToString());
            Assert.Null(result.Order);
        }

    }
}