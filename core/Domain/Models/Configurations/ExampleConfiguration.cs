namespace Core.Domain.Models.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExampleConfiguration : AuditableConfiguration<Example>
{
    public override void Configure(EntityTypeBuilder<Example> builder)
    {
        base.Configure(builder);
    }
}
