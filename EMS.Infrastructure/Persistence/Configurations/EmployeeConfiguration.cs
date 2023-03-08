using EMS.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Infrastructure.Persistence.Configurations;

#nullable disable
public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
        builder.Property(t => t.Email).HasMaxLength(100).IsRequired();
        builder.Property(t => t.DOB).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


