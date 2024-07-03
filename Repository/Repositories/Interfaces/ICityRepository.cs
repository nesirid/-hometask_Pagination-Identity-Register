using Domain.Entities;
using Repository.Helpers;

namespace Repository.Repositories.Interfaces
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<IEnumerable<City>> GetAllWithCountryAsync();
        Task<IEnumerable<City>> FilterAsync(string name, string countryName);
        //Task<PaginationResponse<IEnumerable<City>>> GetPaginateDatasAsync(int page, int take);
        Task<IEnumerable<City>> GetPaginateDatasAsync(int page, int take);

    }
}
