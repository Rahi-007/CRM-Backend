public class Client : BaseEntity<Guid>
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; } = string.Empty;
    public required string Phone { get; set; }
    public string? Address { get; set; } = string.Empty;
    public User CreatedBy { get; set; } = null!;
    public User? UpdatedBy { get; set; }
};
