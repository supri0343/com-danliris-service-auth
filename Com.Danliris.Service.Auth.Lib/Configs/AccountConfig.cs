using Com.Danliris.Service.Auth.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Configs
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(p => p.Username).HasMaxLength(255);
            builder.Property(p => p.Password).HasMaxLength(255);

            builder
                .HasOne(p => p.AccountProfile)
                .WithOne(p => p.Account)
                .HasForeignKey<AccountProfile>(p => p.AccountId);

            builder
                .HasMany(p => p.AccountRoles)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId);
        }
    }
}
