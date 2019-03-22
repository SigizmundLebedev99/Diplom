using AutoMapper;
using System.Linq;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.Mapper
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<CreateBranchVM, CreateBranchDTO>();
            CreateMap<GetBranchesVM, GetBranchesDTO>();
            CreateMap<GetItemsVM, GetItemsDTO>();

            CreateMap<RegisterUserDTO, User>();
            CreateMap<CreateInviteVM, CreateInviteDTO>();
            CreateMap<CreateInviteDTO, Invite>().ForMember(e => e.CreatorId, c => c.MapFrom(e => e.FromUserId));
            CreateMap<Invite, UserProject>().ForMember(u => u.UserId, c => c.MapFrom(i => i.ToUserId));
            CreateMap<ChangeStatusDTO, UserProject>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<CreateProjectDTO, Project>().ForMember(e=>e.CreatorId, c=>c.MapFrom(e=>e.UserId));
            CreateMap<CreateProjectVM, CreateProjectDTO>();
            CreateMap<FileDTO, _File>().ReverseMap();

            CreateMap<CreateTaskDTO, _Task>();
            CreateMap<CreateTaskDTO, SubTask>();
            CreateMap<CreateUserStoryDTO, UserStory>();
            CreateMap<CreateWorkItemDTO, Epick>();

            CreateMap<CreateWorkItemDTO, WorkItemDescription>()
                .ForMember(e => e.Files, c => c.MapFrom(e => e.FileIds.Select(x => new WorkItemFile
                {
                    FileId = x
                })))
                .ForMember(e => e.Tags, c => c.MapFrom(e => e.Tags.Select(r => new WorkItemTag
                {
                    Tag = r
                })));

            CreateMap<CreateSprintDTO, Sprint>();
            CreateMap<CreateSprintVM, CreateSprintDTO>();
            CreateMap<Sprint, SprintDTO>();

            CreateMap<User, UserDTO>();
            CreateMap<WorkItemDescription, DescriptionDTO>();

            CreateMap<CreateCommentDTO, Comment>()
                .ForMember(e => e.CreatorId, c => c.MapFrom(e => e.From.Id))
                .ForMember(e=>e.Files,c=>c.Ignore());
            CreateMap<CreateCommentVM, CommentDTO>();
        }
    }
}
