using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
    public readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _appDbContext;
    public TeamService(AppDbContext appDbContext, IMapper mapper, ICurrentUserService currentUser)
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _appDbContext = appDbContext;
    }

    public async Task<List<TeamResDto>> GetAllTeams()
    {
        var query = _appDbContext.Teams
            .Include(t => t.TeamLeader)
            .Include(t => t.Members)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .AsQueryable();

        var teams = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<TeamResDto>>(teams);
    }

    public async Task<List<SelectTeamRes>> SelectTeams()
    {
        return await _appDbContext.Teams
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ProjectTo<SelectTeamRes>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TeamResDto?> GetTeamById(int id)
    {
        Team? team = await _appDbContext.Teams
            .Include(u => u.TeamLeader)
            .FirstOrDefaultAsync(u => u.Id == id);

        return team == null ? null : _mapper.Map<TeamResDto>(team);
    }

    public async Task<int> CreateTeam(CreateTeamDto createData)
    {
        Team? existTeam = await _appDbContext.Teams.FirstOrDefaultAsync(t => t.Name == createData.Name);
        if (existTeam != null) throw new Exception("Team name is already Exist");

        bool isAlreadyLeader = await _appDbContext.Teams.AnyAsync(t => t.TeamLeaderId == createData.TeamLeaderId);
        if (isAlreadyLeader) throw new Exception("This user is already a team leader.");

        User leader = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == createData.TeamLeaderId)
            ?? throw new Exception("User not found.");

        Team newTeam = _mapper.Map<Team>(createData);
        newTeam.CreatedById = _currentUser.UserId;

        await _appDbContext.Teams.AddAsync(newTeam);
        await _appDbContext.SaveChangesAsync();

        leader.TeamId = newTeam.Id;
        leader.UpdatedById = _currentUser.UserId;
        leader.UpdatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return newTeam.Id;
    }

    public async Task<bool> UpdateTeam(int id, CreateTeamDto updateData)
    {
        Team? team = await _appDbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);

        if (team == null) return false;
        if (!string.IsNullOrWhiteSpace(updateData.Name))
        {
            bool nameExists = await _appDbContext.Teams
                .AnyAsync(u =>
                    u.Name == updateData.Name &&
                    u.Id != id);

            if (nameExists)
                throw new Exception("Name already exists.");
        }


        bool isAlreadyLeader = await _appDbContext.Teams
            .AnyAsync(t => t.TeamLeaderId == updateData.TeamLeaderId && t.Id != id);

        if (isAlreadyLeader)
            throw new Exception("This user is already a team leader.");

        User leader = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == updateData.TeamLeaderId)
            ?? throw new Exception("User not found.");

        if (team.TeamLeaderId != updateData.TeamLeaderId)
        {
            var oldLeader = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == team.TeamLeaderId);

            if (oldLeader != null)
            {
                oldLeader.TeamId = null;
                oldLeader.UpdatedById = _currentUser.UserId;
                oldLeader.UpdatedAt = DateTime.UtcNow;
            }

            leader.TeamId = id;
            leader.UpdatedById = _currentUser.UserId;
            leader.UpdatedAt = DateTime.UtcNow;
        }

        _mapper.Map(updateData, team);
        team.UpdatedById = _currentUser.UserId;
        team.UpdatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTeam(int id)
    {
        Team? team = await _appDbContext.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null) return false;
        foreach (var member in team.Members)
        {
            member.TeamId = null;
            member.UpdatedById = _currentUser.UserId;
            member.UpdatedAt = DateTime.UtcNow;
        }

        _appDbContext.Teams.Remove(team);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}