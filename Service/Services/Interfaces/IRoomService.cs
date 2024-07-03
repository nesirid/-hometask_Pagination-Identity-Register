using Service.DTOs.Admin.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IRoomService
    {
        Task CreateAsync(RoomCreateDto model);
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, RoomEditDto model);
        Task DeleteAsync(int id);
    }
}
