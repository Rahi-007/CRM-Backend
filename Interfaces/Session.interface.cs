public interface IUserSessionService
{
    Task CreateSessionAsync(
        Guid userId,
        string connectionId);

    Task UpdateHeartbeatAsync(
        string connectionId);

    Task EndSessionAsync(
        string connectionId);

    Task<List<OnlineUserResDto>> GetOnlineUsers();
}
