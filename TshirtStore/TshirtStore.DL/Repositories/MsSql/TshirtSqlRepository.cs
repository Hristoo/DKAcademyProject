using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ThirtStore.Models.Models;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MsSql
{
    public class TshirtSqlRepository : ITshirtRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TshirtSqlRepository> _logger;

        public TshirtSqlRepository(IConfiguration configuration, ILogger<TshirtSqlRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Tshirt?> AddTshirt(Tshirt tshirt)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO Tshirt (Name, Size, Color, Price, Quantity) VALUES(@Name, @Size, @Color, @Price, @Quantity)", tshirt);
                    return tshirt;

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddTshirt)}: {e.Message}", e.Message);
            }
            return null;
        }

        public async Task<Tshirt?> DeleteThirt(int tshirtId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<Tshirt>("DELETE FROM Tshirt WHERE Id = @Id", new { Id = tshirtId });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteThirt)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Tshirt> GetTshirtsById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Tshirt>("SELECT * FROM Tshirt WITH(NOLOCK)WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllTshirts)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Tshirt>> GetAllTshirts()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryAsync<Tshirt>("SELECT * FROM Tshirt WITH(NOLOCK)");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllTshirts)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Tshirt> UpdateThirt(Tshirt tshirt)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("UPDATE Tshirt SET Color = @Color,  Size = @Size, Price = @Price, Quantity = @Quantity WHERE Id = @Id", tshirt);
                    return tshirt;
                }

            }
            catch (Exception e)
            {

               _logger.LogError($"Error in {nameof(UpdateThirt)}: {e.Message}", e.Message);
            }
            return null;
        }
    }
}
