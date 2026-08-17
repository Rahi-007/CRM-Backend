public class CreateRoleDto
{
    public required string Name { get; set; }
    public string? Narration { get; set; } = string.Empty;
    public List<int> PermissionIds { get; set; } = new();
};

public class PermissionResDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Module { get; set; } = null!;
    public string Action { get; set; } = null!;
}

public class RoleResDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Narration { get; set; }
    public List<Permission> Permissions { get; set; } = new();
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

public class SelectRoleRes
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
};