using AutoMapper;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectDto, Project>();
        CreateMap<Project, ProjectResDto>();
        CreateMap<Project, SelectProjectRes>();
        CreateMap<User, ProjectResDto.UserRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}")
            );
        CreateMap<Client, ProjectResDto.ClientRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}")
            );
    }
}