using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TshirtStore.HealthChecks
{
    public class CustomExeption : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {

            }
            catch (SqlException e)
            {
                return HealthCheckResult.Unhealthy(e.Message);
            }

            return HealthCheckResult.Healthy("Costom Health Check is OK!");
        }
    }
}
