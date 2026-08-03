public class Project : BaseEntity<int>
{
    public required string Name { get; set; }
    public required BusinessUnit BusinessUnit { get; set; }
    public required Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public string? BriefCode { get; set; }
    public required WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public required DateOnly SubmitDate { get; set; }
    public required string SubmitCode { get; set; }
    public required ProjectStatus Status { get; set; }
    public required Guid AssignedToId { get; set; }
    public User AssignedTo { get; set; } = null!;
    public string? Link { get; set; }
}