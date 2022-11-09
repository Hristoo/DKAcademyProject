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
        public async Task Add_Order_Ok()
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
        public async Task Add_Order_BadPath()
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
        public async Task Delete_Order_Ok()
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
            var orderToDel = _mapper.Map<Order>(orderRequest);
            orderToDel.Id = 2;

            _orderRepositoryMock.Setup(o => o.GetOrderById(orderToDel.Id)).ReturnsAsync(_orders.FirstOrDefault(o => o.Id == orderToDel.Id));
            _orderRepositoryMock.Setup(o => o.DeleteOrder(orderToDel.Id)).Callback(() =>
            {
                _orders.Remove(orderToDel);
            }).ReturnsAsync(orderToDel);

            var handler = new DeleteOrderCommandHandler(_orderRepositoryMock.Object, _mapper);
            var result = await handler.Handle(new DeleteOrderCommand(orderToDel.Id), new CancellationToken());

            Assert.Equal("OK", result.HttpStatusCode.ToString());
            Assert.Equal("Order was deleted successfully!", result.Message);
            Assert.Equal(1, ordCount - 1);
            Assert.NotNull(result.Order);
        }

        [Fact]
        public async Task Delete_Order_BadPath()
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
            var orderToDel = _mapper.Map<Order>(orderRequest);
            orderToDel.Id = 3;

            _orderRepositoryMock.Setup(o => o.GetOrderById(orderToDel.Id)).ReturnsAsync(() => _orders.FirstOrDefault(o => o.Id == orderToDel.Id));
            _orderRepositoryMock.Setup(o => o.DeleteOrder(orderToDel.Id)).Callback(() =>
            {
                _orders.Remove(orderToDel);
            }).ReturnsAsync(orderToDel);

            var handler = new DeleteOrderCommandHandler(_orderRepositoryMock.Object, _mapper);
            var result = await handler.Handle(new DeleteOrderCommand(orderToDel.Id), new CancellationToken());

            Assert.Equal("BadRequest", result.HttpStatusCode.ToString());
            Assert.Equal("Order doesn't exist!", result.Message);
            Assert.Equal(2, ordCount);
            Assert.Null(result.Order);
        }

        [Fact]
        public async Task Get_Order_By_Id_Ok()
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
            var order = _mapper.Map<Order>(orderRequest);
            order.Id = 2;

            _orderRepositoryMock.Setup(o => o.GetOrderById(order.Id)).ReturnsAsync(order);

            var handler = new GetOrderByIdCommandHandler(_orderRepositoryMock.Object);
            var result = await handler.Handle(new GetOrderByIdCommand(order.Id), new CancellationToken());

            Assert.Equal(2, result.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_Order_By_Id_BadPath()
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
            var order = _mapper.Map<Order>(orderRequest);
            order.Id = 3;

            _orderRepositoryMock.Setup(o => o.GetOrderById(order.Id)).ReturnsAsync(order);

            var handler = new GetOrderByIdCommandHandler(_orderRepositoryMock.Object);
            var result = await handler.Handle(new GetOrderByIdCommand(order.Id), new CancellationToken());

            Assert.Null(result);
        }

        [Fact]
        public async Task Update_Order_Ok()
        {
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 55,
                Tshirts = new List<Tshirt>()
                    {
                        new Tshirt()
                        {
                            Id = 55,
                            Color = "Pink",
                        },
                        new Tshirt()
                        {
                            Id = 15,
                            Color = "Pink",
                        }
                    }
            };

            var order = _mapper.Map<Order>(orderRequest);
            order.Id = 2;

            _orderRepositoryMock.Setup(o => o.UpdateOrder(It.IsAny<Order>())).Callback(() =>
            {
                var ord = _orders.FirstOrDefault(o => o.Id == order.Id);
                ord.Sum = order.Sum;
                ord.Tshirts = order.Tshirts;
                ord.ClientId = order.ClientId;
                ord.LastUpdated = order.LastUpdated;
                ord.Id = order.Id;
            }).ReturnsAsync(_orders.FirstOrDefault(o => o.Id == order.Id));

            _orderRepositoryMock.Setup(o => o.GetOrderById(order.Id)).ReturnsAsync(() => _orders.FirstOrDefault(o => o.Id == order.Id));

            var handler = new UpdateOrderCommandHandler(_orderRepositoryMock.Object);
            var result = await handler.Handle(new UpdateOrderCommand(order), new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal("OK", result.HttpStatusCode.ToString());
            Assert.Equal("Order is updated successfully!", result.Message);
        }

        [Fact]
        public async Task Update_Order_BadPath()
        {
            var orderRequest = new OrderRequest()
            {
                ClientId = 25,
                LastUpdated = DateTime.UtcNow,
                Sum = 55,
                Tshirts = new List<Tshirt>()
                    {
                        new Tshirt()
                        {
                            Id = 55,
                            Color = "Pink",
                        },
                        new Tshirt()
                        {
                            Id = 15,
                            Color = "Pink",
                        }
                    }
            };

            var order = _mapper.Map<Order>(orderRequest);
            order.Id = 3;

            _orderRepositoryMock.Setup(o => o.UpdateOrder(It.IsAny<Order>())).Callback(() =>
            {
                var ord = _orders.FirstOrDefault(o => o.Id == order.Id);
                ord.Sum = order.Sum;
                ord.Tshirts = order.Tshirts;
                ord.ClientId = order.ClientId;
                ord.LastUpdated = order.LastUpdated;
                ord.Id = order.Id;
            }).ReturnsAsync(_orders.FirstOrDefault(o => o.Id == order.Id));

            _orderRepositoryMock.Setup(o => o.GetOrderById(order.Id)).ReturnsAsync(() => _orders.FirstOrDefault(o => o.Id == order.Id));

            var handler = new UpdateOrderCommandHandler(_orderRepositoryMock.Object);
            var result = await handler.Handle(new UpdateOrderCommand(order), new CancellationToken());

            Assert.Null(result.Order);
            Assert.Equal("BadRequest", result.HttpStatusCode.ToString());
            Assert.Equal("Order doesn't exist!", result.Message);
        }
    }
}