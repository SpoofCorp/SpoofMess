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

    public virtual DbSet<SessionInfo> SessionInfos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<UserEntry> UserEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("OutboxStatus", ["Pending", "Error", "Success", "Rejected", "Deleting"]);

        modelBuilder.Entity<SessionInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SessionInfo_pkey");

            entity.ToTable("SessionInfo");

            entity.HasIndex(e => e.Id, "IX_SessionInfo_Id");

            entity.HasIndex(e => e.UserEntryId, "IX_SessionInfo_UserId_Active").HasFilter("((\"IsActive\" = true) AND (\"IsDeleted\" = false))");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DeviceId).HasMaxLength(100);
            entity.Property(e => e.DeviceName).HasMaxLength(255);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastActivityAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Platform).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.UserEntry).WithMany(p => p.SessionInfos)
                .HasForeignKey(d => d.UserEntryId)
                .HasConstraintName("FK_SessionInfo_UserEntryId");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Token_pkey");

            entity.ToTable("Token");

            entity.HasIndex(e => e.SessionInfoId, "IX_Token_SessionInfoId").HasFilter("(\"IsDeleted\" = false)");

            entity.HasIndex(e => e.Id, "IX_Token_ValidTo").HasFilter("(\"IsDeleted\" = false)");

            entity.Property(e => e.Id).HasMaxLength(100);

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

        modelBuilder.Entity<UserEntryOutbox>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserEntryOutbox_Id");

            entity.ToTable("UserEntryOutbox");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Data).HasColumnType("jsonb");
            entity.Property(e => e.LastTryDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.UserEntry).WithMany(p => p.UserEntryOutboxes)
                .HasForeignKey(d => d.UserEntryId)
                .HasConstraintName("FK_UserEntryOutbox_UserEntryId");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
