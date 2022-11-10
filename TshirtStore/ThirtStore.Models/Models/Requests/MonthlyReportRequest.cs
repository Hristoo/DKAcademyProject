namespace ThirtStore.Models.Models.Requests
{
    public class MonthlyReportRequest
    {
        public string Month { get; set; }

        public decimal Incomes { get; set; }

        public DateTime Updated { get; set; }
    }
}
