using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Net.Sockets;
using ThirtStore.Models.Models;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MsSql
{
    public class OrderSqlRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderSqlRepository> _logger;

        public OrderSqlRepository(IConfiguration configuration, ILogger<OrderSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Order?> AddOrder(Order order)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryFirstOrDefaultAsync<Order>("INSERT INTO [Order] (ClientId, LastUpdated, Sum) output INSERTED.* VALUES(@ClientId, @LastUpdated, @Sum)", order);
                    order.Id = result.Id;
                    result = await LinkOrderAndTshirts(order);

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddOrder)}: {e.Message}", e.Message);
            }
            return null;
        }


        public async Task<Order?> DeleteOrder(int orderId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<Order>("DELETE FROM [Order] output DELETED.* WHERE Id = @Id", new { Id = orderId });

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteOrder)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<Order>("SELECT * FROM [Order] WITH(NOLOCK) WHERE Id = @Id", new { Id = id });
                    result.Tshirts = await GetTshirtsByOrderId(id);

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetOrderById)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync("UPDATE [Order] SET ClientId = @ClientId, LastUpdated = @LastUpdated, Sum = @Sum WHERE Id = @Id", order);

                    return order;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateOrder)}: {e.Message}", e.Message);
            }
            return null;
        }

        public async Task<List<Order>?> GetOrderByClientId(int clientId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryFirstOrDefaultAsync<List<Order>>("SELECT * FROM [Order] WITH(NOLOCK) WHERE Id = @Id", new { Id = clientId });

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetOrderByClientId)}: {e.Message}", e.Message);
            }

            return null;
        }
        public async Task<Order?> LinkOrderAndTshirts(Order order)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    foreach (var tshirt in order.Tshirts)
                    {
                        var addOrderTshirts = await conn.ExecuteAsync("INSERT INTO [OrdersAndTshirts] (OrderId, TshirtId) output INSERTED.* VALUES(@OrderId, @TshirtId)", new { OrderId = order.Id, TshirtId = tshirt.Id });
                    }

                    return order;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(LinkOrderAndTshirts)}: {e.Message}", e.Message);
            }
            return null;
        }

        public async Task<List<Tshirt>?> GetTshirtsByOrderId(int orderId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var addOrderTshirts = await conn.QueryAsync<Tshirt>("SELECT * FROM [OrdersAndTshirts] as ot INNER JOIN [Tshirt] as t ON ot.TshirtId = t.Id WHERE @OrderId = OrderId", new { OrderId = orderId });

                    return addOrderTshirts.ToList();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetTshirtsByOrderId)}: {e.Message}", e.Message);
            }
            return null;
        }
    }
}
