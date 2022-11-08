using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MsSql
{
    public class ReportSqlRepository : IReportSqlRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientRepository> _logger;

        public ReportSqlRepository(ILogger<ClientRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<MonthlyReportRequest?> AddReport(MonthlyReportRequest report)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    var result = await conn.ExecuteAsync("INSERT INTO [MonthlyReport] (Month, Incomes) output INSERTED.* VALUES(@Month, @Incomes)", report);
                    return report;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddReport)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<MonthlyReportRequest?> UpdateReport(MonthlyReportRequest report)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteAsync("UPDATE MonthlyReport SET Incomes = @Incomes", report);

                    return report;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateReport)}: {e.Message}", e.Message);
            }

            return null;
        }

        public async Task<decimal> GetMonthlyIncomes(string month)
        {
            decimal incomes = 0;

            if (month == DateTime.Now.ToString("MMM"))
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    incomes = await conn.QueryFirstOrDefaultAsync<decimal>("SELECT Incomes From [MonthlyReport] WITH(NOLOCK) WHERE Month = @Month", new { Month = month });
                    
                    return incomes;
                }
            }

            return incomes;
        }
    }
}
