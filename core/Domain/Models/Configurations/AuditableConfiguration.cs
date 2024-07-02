namespace Core.Domain.Models.Configurations;

using Core.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class AuditableConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class, IAuditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.CreatedBy).HasMaxLength(400);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(400);
    }
}
