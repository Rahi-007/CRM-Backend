public class Role : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string? Narration { get; set; }
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}