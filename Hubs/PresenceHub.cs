using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

[Authorize]
public class PresenceHub : Hub
{
    private readonly IUserSessionService _userSessionService;

    public PresenceHub(
        IUserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
    }

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            Context.Abort();
            return;
        }

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            Context.Abort();
            return;
        }

        await _userSessionService.CreateSessionAsync(
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public async Task Heartbeat()
    {
        await _userSessionService.UpdateHeartbeatAsync(
            Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception)
    {
        await _userSessionService.EndSessionAsync(
            Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
