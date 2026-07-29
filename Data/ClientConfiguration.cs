using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(u => u.UpdatedBy)
            .WithMany()
            .HasForeignKey(u => u.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}