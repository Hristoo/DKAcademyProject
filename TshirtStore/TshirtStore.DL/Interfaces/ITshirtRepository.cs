using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;

namespace TshirtStore.DL.Interfaces
{
    public interface ITshirtRepository
    {
        public Task<IEnumerable<Tshirt>> GetAllTshirts();

        public Task<Tshirt?> AddTshirt(Tshirt tshirt);

        public Task<Tshirt?> DeleteThirt(int tshirtId);

        public Task<Tshirt> UpdateThirt(Tshirt tshirt);
    }
}
