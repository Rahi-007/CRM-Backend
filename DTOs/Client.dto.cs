public class CreateClientDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; } = string.Empty;
    public required string Phone { get; set; }
    public string? Address { get; set; } = string.Empty;

};

public class ClientResDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; } = string.Empty;
    public required string Phone { get; set; }
    public string? Address { get; set; } = string.Empty;
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

public class SelectClientRes
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public required string Phone { get; set; }
};