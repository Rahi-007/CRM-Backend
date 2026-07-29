public interface IClientService
{
    Task<List<ClientResDto>> GetAllClients();
    Task<List<SelectClientRes>> SelectClients();
    Task<ClientResDto?> GetClientById(Guid clientId);
    Task<Guid> CreateClient(CreateClientDto createData);
    Task<bool> UpdateClient(Guid clientId, CreateClientDto updateData);
    Task<bool> DeleteClient(Guid id);
}