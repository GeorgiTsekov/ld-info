using AutoMapper;
using LDInfo.Api.Features.Projects.Model;
using LDInfo.Api.Features.TimeLogs.Models;
using LDInfo.Api.Features.Users.Models;
using LDInfo.Data.Entities;

namespace LDInfo.Api.Features
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, Project>().ReverseMap();
            CreateMap<TimeLogDto, TimeLog>().ReverseMap();
            CreateMap<TimeLog, TimeLogDto>().ReverseMap();
            CreateMap<TimeLog, TimeLogDetails>().ReverseMap();
            CreateMap<TimeLog, TopUserDetails>().ReverseMap();
            CreateMap<TopUserDetails, TimeLog>().ReverseMap();
        }
    }
}
