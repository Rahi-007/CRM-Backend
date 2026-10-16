public class OnlineUserResDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime LastSeenAt { get; set; }
}