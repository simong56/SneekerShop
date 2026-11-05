using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Models;

public partial class AuthUserContext : DbContext
{
    public AuthUserContext()
    {
    }

    public AuthUserContext(DbContextOptions<AuthUserContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.RegTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Password)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
            entity.Property(e => e.RegTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PRIMARY");
                        j.ToTable("UserRole");
                        j.HasIndex(new[] { "RoleId" }, "RoleId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
