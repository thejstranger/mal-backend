using AutoMapper;
using MAL.DAL;
using MAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.Serivces
{
    public class UserServiceMappings : Profile
    {
        public UserServiceMappings()
        {
            CreateMap<Users, UserDto>().ReverseMap();
        }
    }
}
