public class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? ConnectionId { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? LastSeenAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public bool IsActive { get; set; }

    public User User { get; set; } = null!;
}