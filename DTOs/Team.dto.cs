public class CreateTeamDto
{
    public required string Name { get; set; }
    public string? Narration { get; set; } = string.Empty;
    public Guid TeamLeaderId { get; set; }
};

public class TeamResDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Narration { get; set; } = string.Empty;
    public UserRes TeamLeader { get; set; } = null!;
    public ICollection<UserRes> Members { get; set; } = [];
    public UserRes CreatedBy { get; set; } = null!;
    public UserRes? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public class UserRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
};

public class SelectTeamRes
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
};