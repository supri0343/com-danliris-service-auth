using Com.Danliris.Service.Auth.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(255);
            builder.Property(p => p.Name).HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(3000);

            builder
                .HasMany(p => p.Permissions)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);

            builder
                .HasMany(p => p.AccountRoles)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);
        }
    }
}
