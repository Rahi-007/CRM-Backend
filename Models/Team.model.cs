public class Team : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? Narration { get; set; } = string.Empty;
    public ICollection<User> Members { get; set; } = [];
    public Guid TeamLeaderId { get; set; }
    public User TeamLeader { get; set; } = null!;
};
