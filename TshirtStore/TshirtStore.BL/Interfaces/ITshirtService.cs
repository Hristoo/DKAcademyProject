using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;

namespace TshirtStore.BL.Interfaces
{
    public interface ITshirtService
    {
        public Task<IEnumerable<Tshirt>> GetAllTshirts();
        public Task<TshirtResponse?> AddThirt(TshirtRequest tshirt);

        public Task<Tshirt?> DeleteThirt(int tshirtId);

        public Task<Tshirt> UpdateThirt(Tshirt tshirt);

    }
}
