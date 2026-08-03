public class CreateUserDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; } = string.Empty;
    public required string Phone { get; set; }
    public string? Address { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public int? TeamId { get; set; } = null;
    public string? RFId { get; set; } = string.Empty;
    public required string Password { get; set; }
};


public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public int? TeamId { get; set; }
    public string? RFId { get; set; }
}

public class ChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

public class UserResDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = null!;
    public string? Address { get; set; } = string.Empty;
    public TeamRes? Team { get; set; } = null;
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? RFId { get; set; } = string.Empty;
    public ICollection<ProjectRes> AssignedProjects { get; set; } = [];
    public UserRes CreatedBy { get; set; } = null!;
    public UserRes? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public class TeamRes
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
    public class ProjectRes
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
    public class UserRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

public class SelectUserRes
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = null!;
    public string Name { get; set; } = null!;
}