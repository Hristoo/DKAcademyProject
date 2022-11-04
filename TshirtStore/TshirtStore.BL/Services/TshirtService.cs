using AutoMapper;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;
using TshirtStore.BL.Interfaces;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.Services
{
    public class TshirtService : ITshirtService
    {
        private readonly ITshirtRepository _tshirtRepository;
        private readonly IMapper _mapper;

        public TshirtService(ITshirtRepository tshirtRepository, IMapper mapper)
        {
            _tshirtRepository = tshirtRepository;
            _mapper = mapper;
        }

        public async Task<TshirtResponse?> AddThirt(TshirtRequest tshirtRequest)
        {
            var tshirt = _mapper.Map<Tshirt>(tshirtRequest);
            var result = await _tshirtRepository.AddTshirt(tshirt);

            return new TshirtResponse()
            {
                Tshirt = result,
            };
        }

        public async Task<Tshirt?> DeleteThirt(int tshirtId)
        {
            return await _tshirtRepository.DeleteThirt(tshirtId);
        }

        public Task<IEnumerable<Tshirt>> GetAllTshirts()
        {
            return _tshirtRepository.GetAllTshirts();
        }

        public async Task<Tshirt> UpdateThirt(Tshirt tshirt)
        {
            return await _tshirtRepository.UpdateThirt(tshirt);
        }
    }
}
