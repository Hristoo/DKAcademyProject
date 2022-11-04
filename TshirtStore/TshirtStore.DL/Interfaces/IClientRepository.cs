using ThirtStore.Models.Models;

namespace TshirtStore.DL.Interfaces
{
    public interface IClientRepository
    {
        public Task<Client?> AddClient(Client client);
        public Task<Client?> DeleteClient(int clientId);
        public Task<IEnumerable<Client>> GetAllClients();
        public Task<Client?> GetById(int id);
        public Task<Client> GetClientByName(string name);
        public Task<Client> UpdateClient(Client client);
    }
}
