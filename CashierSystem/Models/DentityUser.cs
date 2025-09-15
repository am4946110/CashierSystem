using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CashierSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<MyUser, MyRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // تخصيص أسماء الجداول والـ Schema
            builder.Entity<MyUser>().ToTable("MyUsers", "SUR");
            builder.Entity<MyRole>().ToTable("MyRoles", "SUR");
            builder.Entity<DeviceInfo>().ToTable("DeviceInfo", "SUR");
            builder.Entity<ProtectionKey>().ToTable("ProtectionKeys", "key");
        }

        public DbSet<MyRole> MyRoles { get; set; }
        public DbSet<MyUser> MyUsers { get; set; }
        public DbSet<ProtectionKey> ProtectionKeys { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceInfo> DeviceInfo { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ViewModels.RoleModel> RoleModels { get; set; } = default!;
        public DbSet<ViewModels.EditPasswordModel> EditPasswords { get; set; }
        public DbSet<ViewModels.RegisterModel> RegisterModels { get; set; } = default!;
        public DbSet<ViewModels.UserRoleModel> UserRoleModels { get; set; } = default!;
        public object MyUser { get; internal set; }
        public IEnumerable<object> Products { get; internal set; }
      
    }






    [Table("MyRole", Schema = "SUR")]
    public class MyRole : IdentityRole
    {
        public MyRole() { }

        public MyRole(string roleName) : base(roleName)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpperInvariant();
        }

        [NotMapped]
        public string EncryptedId { get; set; }
    }


    [Table("DeviceInfo", Schema = "SUR")]

    public class DeviceInfo
    {
        [Key]
        public Guid usersId { get; set; } = Guid.NewGuid();

        public string IPAddress { get; set; }
        public string HostName { get; set; }
        public string Status { get; set; }
    }


    [Table("MyUser", Schema = "SUR")]
    public class MyUser : IdentityUser
    {
        [NotMapped]
        public string usersId { get; set; }
        public string? Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[]? ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public virtual ICollection<Sale> Sale { get; set; } = new List<Sale>();
    }

    [Table("ProtectionKey", Schema = "key")]

    public class ProtectionKey : IKey
    {
        [Key]
        public Guid KeyId { get; set; }
        public string Value { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset? RevocationDate { get; set; }
        public string? RevocationReason { get; set; }

        DateTimeOffset IKey.ActivationDate => throw new NotImplementedException();

        DateTimeOffset IKey.CreationDate => throw new NotImplementedException();

        DateTimeOffset IKey.ExpirationDate => throw new NotImplementedException();

        bool IKey.IsRevoked => throw new NotImplementedException();

        Guid IKey.KeyId => throw new NotImplementedException();

        IAuthenticatedEncryptorDescriptor IKey.Descriptor => throw new NotImplementedException();

        IAuthenticatedEncryptor? IKey.CreateEncryptor()
        {
            throw new NotImplementedException();
        }
    }
}

