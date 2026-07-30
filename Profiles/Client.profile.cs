using AutoMapper;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<CreateClientDto, Client>();
        CreateMap<User, ClientResDto.UserRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}")
            );
        CreateMap<Client, ClientResDto>();
        CreateMap<Client, SelectClientRes>()
            .ForMember(
                d => d.Name,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.LastName)
                        ? s.FirstName
                        : $"{s.FirstName} {s.LastName}"));
    }
}