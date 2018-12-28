﻿using AutoMapper;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.Mapper
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<CreateInviteVM, CreateInviteDTO>();
            CreateMap<CreateInviteDTO, Invite>().ForMember(e => e.CreatorId, c => c.MapFrom(e => e.FromUserId));
            CreateMap<Invite, UserProject>().ForMember(u => u.UserId, c => c.MapFrom(i => i.ToUserId));
            CreateMap<ChangeStatusDTO, UserProject>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<CreateProjectDTO, Project>().ForMember(e=>e.CreatorId, c=>c.MapFrom(e=>e.UserId));
            CreateMap<CreateProjectVM, CreateProjectDTO>().ForMember(e => e.Logo, c => c.Ignore());
        }
    }
}
