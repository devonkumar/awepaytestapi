using AutoMapper;
using AwePay.CQRS;
using AwePay.Domains;
using AwePay.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Mapper
{
    public class MemberProfile : Profile
    {

        public MemberProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<MemCreate, User>();

        }
    }
}
