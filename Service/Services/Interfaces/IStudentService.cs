using Service.DTOs.Admin.Students;

namespace Service.Services.Interfaces
{
    public interface IStudentService
    {
        Task CreateAsync(StudentCreateDto model);
        Task EditAsync(int id, StudentEditDto model);
        Task<StudentDto> GetByIdWithInclude(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<StudentDto>> GetAllWithInclude();
    }
}
