using System;
using System.Collections.Generic;
using MAL.DTO;

namespace MAL.Services
{
    public interface IUserService
    {
        List<UserDto> GetAllUsers();

        string LoginUser(UserDto userLogin);
    }
}
