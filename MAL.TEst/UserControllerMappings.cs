using AutoMapper;
using MAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAL.TEst
{
    public class UserControllerMappings : Profile
    {
        public UserControllerMappings()
        {
            CreateMap<UserLoginViewModel, UserDto>().ReverseMap();
        }
    }
}
