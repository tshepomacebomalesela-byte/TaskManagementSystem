using AutoMapper;
using TaskApplication.Tasks.DTOs;

namespace TaskApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskDomain.Task, TaskDTO>()
                .ForMember(dest => dest.StatusName,
                           opt => opt.MapFrom(src => src.Status.Name));
        }
    }
}
