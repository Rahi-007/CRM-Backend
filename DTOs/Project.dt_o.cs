public class CreateProjectDto
{
    public required string Name { get; set; }
    public required BusinessUnit BusinessUnit { get; set; }
    public required Guid ClientId { get; set; }
    public string? BriefCode { get; set; }
    public required WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public required DateOnly SubmitDate { get; set; }
    public required string SubmitCode { get; set; }
    public required ProjectStatus Status { get; set; }
    public required Guid AssignedToId { get; set; }
    public string? Link { get; set; }
};

public class ProjectResDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public BusinessUnit BusinessUnit { get; set; }
    public ClientRes Client { get; set; } = null!;
    public UserRes AssignedTo { get; set; } = null!;
    public string? BriefCode { get; set; }
    public WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public DateOnly SubmitDate { get; set; }
    public string SubmitCode { get; set; } = null!;
    public ProjectStatus Status { get; set; }
    public string? Link { get; set; }
    public UserRes CreatedBy { get; set; } = null!;
    public UserRes? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public class UserRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
    public class ClientRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
};

public class SelectProjectRes
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
};