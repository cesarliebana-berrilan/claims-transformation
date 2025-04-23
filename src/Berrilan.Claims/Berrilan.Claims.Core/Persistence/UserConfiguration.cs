using Berrilan.Claims.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Berrilan.Claims.Core.Persistence;

public class UserConfiguration() : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(entity => entity.Email).HasMaxLength(200).IsUnicode().IsRequired();
        builder.Property(entity => entity.CustomerId).IsRequired();
        builder.Property(entity => entity.IsRoot).IsRequired();
        builder.Property(entity => entity.Role).HasMaxLength(20).IsRequired().IsUnicode(false).HasConversion<string>();
    }
}
