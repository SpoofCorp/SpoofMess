using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpoofEntranceService.Models;

public partial class SpoofEntranceServiceContext : DbContext
{
    public SpoofEntranceServiceContext()
    {
    }

    public SpoofEntranceServiceContext(DbContextOptions<SpoofEntranceServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OperationStatus> OperationStatuses { get; set; }

    public virtual DbSet<SessionInfo> SessionInfos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<UserEntry> UserEntries { get; set; }

    public virtual DbSet<UserEntryOperationStatus> UserEntryOperationStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<OperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OperationStatus_Id");

            entity.ToTable("OperationStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SessionInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SessionInfo_pkey");

            entity.ToTable("SessionInfo");

            entity.HasIndex(e => e.Id, "IX_SessionInfo_Id");

            entity.HasIndex(e => e.UserEntryId, "IX_SessionInfo_UserId_Active").HasFilter("((\"IsActive\" = true) AND (\"IsDeleted\" = false))");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeviceId).HasMaxLength(100);
            entity.Property(e => e.DeviceName).HasMaxLength(255);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastActivityAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Platform).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.UserEntry).WithMany(p => p.SessionInfos)
                .HasForeignKey(d => d.UserEntryId)
                .HasConstraintName("FK_SessionInfo_UserEntryId");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenHash).HasName("Token_pkey");

            entity.ToTable("Token");

            entity.HasIndex(e => e.SessionInfoId, "IX_Token_SessionInfoId").HasFilter("(\"IsDeleted\" = false)");

            entity.HasIndex(e => e.RefreshTokenHash, "IX_Token_ValidTo").HasFilter("(\"IsDeleted\" = false)");

            entity.Property(e => e.RefreshTokenHash).HasMaxLength(100);
            entity.Property(e => e.ValidTo).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.SessionInfo).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.SessionInfoId)
                .HasConstraintName("FK_Token_SessionInfoId");
        });

        modelBuilder.Entity<UserEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserEntry_pkey");

            entity.ToTable("UserEntry");

            entity.HasIndex(e => e.UniqueName, "IX_UserEntry_UniqueName");

            entity.HasIndex(e => e.UniqueName, "UserEntry_UniqueName_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.UniqueName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserEntryOperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserEntryOperationStatus_Id");

            entity.ToTable("UserEntryOperationStatus");

            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.TimeSet)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.OperationStatus).WithMany(p => p.UserEntryOperationStatuses)
                .HasForeignKey(d => d.OperationStatusId)
                .HasConstraintName("FK_UserEntryOperationStatus_OperationStatusId");

            entity.HasOne(d => d.UserEntry).WithMany(p => p.UserEntryOperationStatuses)
                .HasForeignKey(d => d.UserEntryId)
                .HasConstraintName("FK_UserEntryOperationStatus_UserEntryId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
