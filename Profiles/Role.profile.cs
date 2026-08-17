using AutoMapper;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Permission, PermissionResDto>();
        CreateMap<User, RoleResDto.UserRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}")
            );
        CreateMap<Role, RoleResDto>();
        CreateMap<Role, SelectRoleRes>();
        CreateMap<CreateRoleDto, Role>()
            .ForMember(
                    d => d.Permissions,
                    o => o.Ignore()
            );
    }
}