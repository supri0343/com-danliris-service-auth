using Com.Danliris.Service.Auth.Lib.Configs;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Moonlay.Data.EntityFrameworkCore;
using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib
{
    public class AuthDbContext : StandardDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Account>(new AccountConfig());
            modelBuilder.ApplyConfiguration<AccountProfile>(new AccountProfileConfig());
            modelBuilder.ApplyConfiguration<Permission>(new PermissionConfig());
            modelBuilder.ApplyConfiguration<Role>(new RoleConfig());
        }
    }
}
