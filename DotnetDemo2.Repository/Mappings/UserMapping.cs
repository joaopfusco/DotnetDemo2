using DotnetDemo2.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace DotnetDemo2.Repository.Mappings
{
    public class UserMapping : BaseMapping<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.KeycloakId).IsRequired();
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Email).IsRequired();

            builder.HasIndex(x => new { x.KeycloakId }).IsUnique();
            builder.HasIndex(x => new { x.Username }).IsUnique();
            builder.HasIndex(x => new { x.Email }).IsUnique();

            base.Configure(builder);
        }
    }
}
