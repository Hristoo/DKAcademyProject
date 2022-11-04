using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ThirtStore.Models.Models;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MsSql
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(IConfiguration configuration, ILogger<ClientRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<Client?> AddClient(Client client)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO Client (Name, Address) VALUES(@Name, @Address)", client);
                    return client;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddClient)}: {e.Message}", e.Message);
            }
            return null;
        }

        public async Task<Client?> DeleteClient(int clientId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))

                {
                    await conn.OpenAsync();

                    var client = await conn.QueryFirstOrDefaultAsync<Client>("SELECT * FROM Client WITH(NOLOCK) WHERE Id = @Id", new { Id = clientId });

                    var result = await conn.QueryFirstOrDefaultAsync<Client>("DELETE FROM Client WHERE Id = @Id", new { Id = clientId });

                    return client;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteClient)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            var results = new List<Client>();

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryAsync<Client>("SELECT * FROM Client WITH(NOLOCK)");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllClients)}: {e.Message}", e);
            }
            return results;
        }

        public async Task<Client?> GetById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.QueryFirstOrDefaultAsync<Client>("SELECT * FROM Client WITH(NOLOCK) WHERE Id = @Id", new { Id = id });
                    return result;

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Client> GetClientByName(string name)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<Client>("SELECT * FROM Client WITH(NOLOCK) WHERE Name = @Name", new { Name = name });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetClientByName)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("UPDATE Client SET Name = @Name, Address = @Address WHERE Id = @Id", client);
                    return client;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddClient)}: {e.Message}", e.Message);
            }
            return null;
        }
    }
}
