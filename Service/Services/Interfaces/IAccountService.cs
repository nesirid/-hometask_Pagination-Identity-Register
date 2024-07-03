using Microsoft.EntityFrameworkCore.Internal;
using Service.DTOs.Account;
using Service.Helpers.Account;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponse> SignUpAsync(RegisterDto model);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByUsernameAsync(string username); 

    }
}
