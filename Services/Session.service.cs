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
            .Select(x => new OnlineUserResDto
            {
                Id = x.UserId,

                FullName = string.IsNullOrEmpty(x.User.LastName)
                    ? x.User.FirstName
                    : x.User.FirstName + " " + x.User.LastName,

                LastSeenAt = x.StartedAt!.Value
            })
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task CreateSessionAsync(
    Guid userId,
    string connectionId)
    {
        var now = DateTime.UtcNow;

        var session = await _appDbContext.UserSessions
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (session == null)
        {
            session = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            _appDbContext.UserSessions.Add(session);
        }

        session.ConnectionId = connectionId;
        session.StartedAt = now;
        session.LastSeenAt = now;
        session.EndedAt = null;
        session.IsActive = true;

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

        session.IsActive = false;
        session.EndedAt = DateTime.UtcNow;
        session.LastSeenAt = DateTime.UtcNow;
        session.ConnectionId = null;

        await _appDbContext.SaveChangesAsync();
    }
}