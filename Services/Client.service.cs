using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class ClientService : IClientService
{
    public readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _appDbContext;
    public ClientService(AppDbContext appDbContext, IMapper mapper, ICurrentUserService currentUser)
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _appDbContext = appDbContext;
    }

    public async Task<List<ClientResDto>> GetAllClients()
    {
        var query = _appDbContext.Clients
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .AsQueryable();

        var clients = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<ClientResDto>>(clients);
    }

    public async Task<List<SelectClientRes>> SelectClients()
    {
        return await _appDbContext.Clients
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ProjectTo<SelectClientRes>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ClientResDto?> GetClientById(Guid id)
    {
        Client? client = await _appDbContext.Clients
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(u => u.Id == id);

        return client == null ? null : _mapper.Map<ClientResDto>(client);
    }

    public async Task<Guid> CreateClient(CreateClientDto createData)
    {
        Client? existClient = await _appDbContext.Clients.FirstOrDefaultAsync(t => t.Phone == createData.Phone);
        if (existClient != null) throw new Exception("Phone number is already Exist");

        Client newClient = _mapper.Map<Client>(createData);
        newClient.CreatedById = _currentUser.UserId;

        await _appDbContext.Clients.AddAsync(newClient);
        await _appDbContext.SaveChangesAsync();

        return newClient.Id;
    }

    public async Task<bool> UpdateClient(Guid id, CreateClientDto updateData)
    {
        Client? client = await _appDbContext.Clients.FirstOrDefaultAsync(x => x.Id == id);

        if (client == null) return false;
        if (!string.IsNullOrWhiteSpace(updateData.Phone))
        {
            bool phoneExists = await _appDbContext.Clients
                .AnyAsync(u =>
                    u.Phone == updateData.Phone &&
                    u.Id != id);

            if (phoneExists)
                throw new Exception("Phone number is already exists");
        }

        _mapper.Map(updateData, client);
        client.UpdatedById = _currentUser.UserId;
        client.UpdatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteClient(Guid id)
    {
        Client? client = await _appDbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);

        if (client == null) return false;

        _appDbContext.Clients.Remove(client);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}