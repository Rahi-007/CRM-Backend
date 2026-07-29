using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, UserResDto>();
        CreateMap<User, SelectUserRes>()
        .ForMember(
            d => d.Name,
            o => o.MapFrom(s =>
                s.LastName == null || s.LastName == ""
                    ? s.FirstName
                    : s.FirstName + " " + s.LastName)
        );
        CreateMap<Team, UserResDto.TeamRes>();

        CreateMap<User, UserResDto.UserRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}")
            );
    }
}