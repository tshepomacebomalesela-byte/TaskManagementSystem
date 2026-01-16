using AutoMapper;
using TaskApplication.Tasks.DTOs;

namespace TaskApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<TaskDomain.Task, TaskDTO>()
                // AutoMapper automatically flattens 'Status.Name' to 'StatusName' 
                // by convention. If you wanted to be explicit, you would use:
                .ForMember(dest => dest.StatusName,
                           opt => opt.MapFrom(src => src.Status.Name));
        }
    }
}
