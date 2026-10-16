using Microsoft.EntityFrameworkCore;

public class UserSessionService : IUserSessionService
{
    private readonly AppDbContext _appDbContext;

    public UserSessionService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<OnlineUserResDto>> GetOnlineUsers()
    {
        var timeout = DateTime.UtcNow.AddMinutes(-1);

        return await _appDbContext.UserSessions
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.LastSeenAt >= timeout)
            .GroupBy(x => new
            {
                x.UserId,
                x.User.FirstName,
                x.User.LastName
            })
            .Select(x => new OnlineUserResDto
            {
                Id = x.Key.UserId,

                FullName = string.IsNullOrEmpty(x.Key.LastName)
                    ? x.Key.FirstName
                    : x.Key.FirstName + " " + x.Key.LastName,

                LastSeenAt = x.Max(s => s.LastSeenAt)
            })
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task CreateSessionAsync(
        Guid userId,
        string connectionId)
    {
        var now = DateTime.UtcNow;

        var session = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ConnectionId = connectionId,
            StartedAt = now,
            LastSeenAt = now,
            IsActive = true
        };

        _appDbContext.UserSessions.Add(session);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateHeartbeatAsync(string connectionId)
    {
        var session = await _appDbContext.UserSessions
            .FirstOrDefaultAsync(x =>
                x.ConnectionId == connectionId &&
                x.IsActive);

        if (session == null)
            return;

        session.LastSeenAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
    }

    public async Task EndSessionAsync(string connectionId)
    {
        var session = await _appDbContext.UserSessions
            .FirstOrDefaultAsync(x =>
                x.ConnectionId == connectionId &&
                x.IsActive);

        if (session == null)
            return;

        var now = DateTime.UtcNow;

        session.IsActive = false;
        session.EndedAt = now;
        session.LastSeenAt = now;

        await _appDbContext.SaveChangesAsync();
    }
}