using AutoMapper;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.Mapper
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            this.CreateMap<RegisterUserDTO, User>();
        }
    }
}
