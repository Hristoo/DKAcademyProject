using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static MongoDB.Driver.WriteConcern;
using System.Threading.Tasks;

namespace Kafka.Interfaces
{
    public interface IGenericProducer<TKey, TValue>
    {
        Task SendMessage(TKey key, TValue value);
    }
}
