public abstract class BaseEntity<TKey>
{
    public required TKey Id { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? UpdatedById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public User CreatedBy { get; set; } = null!;
    public User? UpdatedBy { get; set; }
}