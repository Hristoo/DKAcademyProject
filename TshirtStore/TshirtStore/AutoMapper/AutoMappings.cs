using AutoMapper;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<TshirtRequest, Tshirt>();
            CreateMap<ClientRequest, Client>();
            CreateMap<OrderRequest, Order>();
            CreateMap<MonthlyReportRequest, MonthlyReport>();
        }
    }
}
