using Microsoft.EntityFrameworkCore;

namespace SpoofCallService.Models;

public partial class SpoofCallServiceContext : DbContext
{
    public SpoofCallServiceContext()
    {
    }

    public SpoofCallServiceContext(DbContextOptions<SpoofCallServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<CallMember> CallMembers { get; set; }

    public virtual DbSet<CallMemberSession> CallMemberSessions { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("CallMemberStatusType", ["Invited", "Joined", "Left", "Missed", "Declined"])
            .HasPostgresEnum("CallStatusType", ["Ringing", "Active", "Ended", "Cancelled"])
            .HasPostgresEnum("EndReasonType", ["Hangup", "Timeout", "AllLeft", "Error"])
            .HasPostgresEnum("SessionEndReasonType", ["Hangup", "Disconnected", "Kicked", "Error"])
            .HasPostgresEnum("TopologyType", ["Mesh", "SFU"]);

        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Call_Id");

            entity.ToTable("Call");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Chat).WithMany(p => p.Calls)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_Call_ChatId");

            entity.HasOne(d => d.Initiator).WithMany(p => p.Calls)
                .HasForeignKey(d => d.InitiatorId)
                .HasConstraintName("FK_Call_InitiatorId");
        });

        modelBuilder.Entity<CallMember>(entity =>
        {
            entity.HasKey(e => new { e.Key1, e.Key2 }).HasName("PK_CallMember_Id");

            entity.ToTable("CallMember");

            entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Call).WithMany(p => p.CallMembers)
                .HasForeignKey(d => d.Key1)
                .HasConstraintName("FK_CallMember_CallId");

            entity.HasOne(d => d.User).WithMany(p => p.CallMembers)
                .HasForeignKey(d => d.Key2)
                .HasConstraintName("FK_CallMember_UserId");
        });

        modelBuilder.Entity<CallMemberSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CallMemberSession_Id");

            entity.ToTable("CallMemberSession");

            entity.HasIndex(e => new { e.CallId, e.UserId }, "IX_CallMemberSession_Opened").HasFilter("(\"LeftAt\" IS NULL)");

            entity.HasIndex(e => new { e.UserId, e.JoinedAt }, "IX_CallMemberSession_Recent").IsDescending(false, true);

            entity.Property(e => e.DeviceId).HasMaxLength(100);
            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CallMember).WithMany(p => p.CallMemberSessions)
                .HasForeignKey(d => new { d.CallId, d.UserId })
                .HasConstraintName("FK_CallMemberSession_CallMemberId");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Chat_Id");

            entity.ToTable("Chat");

            entity.HasIndex(e => e.UniqueName, "Chat_UniqueName_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UniqueName).HasMaxLength(100);

            entity.HasOne(d => d.Owner).WithMany(p => p.Chats)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Chat_OwnerId");
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => new { e.Key1, e.Key2 }).HasName("PK_ChatUser_Id");

            entity.ToTable("ChatUser");

            entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.Key1)
                .HasConstraintName("FK_ChatUser_ChatId");

            entity.HasOne(d => d.User).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.Key2)
                .HasConstraintName("FK_ChatUser_UserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "User_Login_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
