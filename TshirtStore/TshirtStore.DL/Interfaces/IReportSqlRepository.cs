using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.DL.Interfaces
{
    public interface IReportSqlRepository
    {
        public Task<MonthlyReportRequest?> AddReport(MonthlyReportRequest report);

        public Task<MonthlyReportRequest?> UpdateReport(MonthlyReportRequest report);

        public Task<decimal> GetMonthlyIncomes(string month);
    }
}
