using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TshirtStore.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SqlHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                }
                catch (SqlException e)
                {
                    return HealthCheckResult.Unhealthy(e.Message);
                }

                return HealthCheckResult.Healthy("SQL connecton is OK!");

            }
        }
    }
}
