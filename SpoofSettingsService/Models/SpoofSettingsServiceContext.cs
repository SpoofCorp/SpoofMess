using Microsoft.EntityFrameworkCore;

namespace SpoofSettingsService.Models;

public partial class SpoofSettingsServiceContext : DbContext
{
    public SpoofSettingsServiceContext()
    {
    }

    public SpoofSettingsServiceContext(DbContextOptions<SpoofSettingsServiceContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatAvatar> ChatAvatars { get; set; }

    public virtual DbSet<ChatProperty> ChatProperties { get; set; }

    public virtual DbSet<ChatType> ChatTypes { get; set; }

    public virtual DbSet<ChatTypeChatProperty> ChatTypeChatProperties { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

    public virtual DbSet<ChatUserPermission> ChatUserPermissions { get; set; }

    public virtual DbSet<Extension> Extensions { get; set; }

    public virtual DbSet<FileMetadataOperationStatus> FileMetadataOperationStatuses { get; set; }

    public virtual DbSet<FileMetadatum> FileMetadata { get; set; }

    public virtual DbSet<OperationStatus> OperationStatuses { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Sticker> Stickers { get; set; }

    public virtual DbSet<StickerPack> StickerPacks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAvatar> UserAvatars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Chat_Id");

            entity.ToTable("Chat");

            entity.HasIndex(e => e.ChatUniqueName, "Chat_ChatUniqueName_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ChatName).HasMaxLength(100);
            entity.Property(e => e.ChatUniqueName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.ChatType).WithMany(p => p.Chats)
                .HasForeignKey(d => d.ChatTypeId)
                .HasConstraintName("FK_Chat_ChatTypeId");

            entity.HasOne(d => d.Owner).WithMany(p => p.Chats)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Chat_OwnerId");
        });

        modelBuilder.Entity<ChatAvatar>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.FileId }).HasName("PK_ChatAvatar_Id");

            entity.ToTable("ChatAvatar");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatAvatars)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_ChatAvatar_ChatId");

            entity.HasOne(d => d.File).WithMany(p => p.ChatAvatars)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_ChatAvatar_FileId");
        });

        modelBuilder.Entity<ChatProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ChatProperty_Id");

            entity.ToTable("ChatProperty");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ChatType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ChatType_Id");

            entity.ToTable("ChatType");

            entity.HasIndex(e => e.Name, "ChatType_Name_key").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ChatTypeChatProperty>(entity =>
        {
            entity.HasKey(e => new { e.ChatTypeId, e.ChatPropertyId }).HasName("PK_ChatTypeChatProperty_Id");

            entity.ToTable("ChatTypeChatProperty");

            entity.HasOne(d => d.ChatProperty).WithMany(p => p.ChatTypeChatProperties)
                .HasForeignKey(d => d.ChatPropertyId)
                .HasConstraintName("FK_ChatTypeChatProperty_ChatPropertyId");

            entity.HasOne(d => d.ChatType).WithMany(p => p.ChatTypeChatProperties)
                .HasForeignKey(d => d.ChatTypeId)
                .HasConstraintName("FK_ChatTypeChatProperty_ChatTypeId");
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.UserId }).HasName("PK_ChatUser_Id");

            entity.ToTable("ChatUser");

            entity.HasIndex(e => e.ChatId, "IX_ChatUser_ChatId").HasFilter("(\"IsDeleted\" = false)");

            entity.HasIndex(e => new { e.UserId, e.ChatId }, "IX_ChatUser_UserChat").HasFilter("(\"IsDeleted\" = false)");

            entity.HasIndex(e => new { e.ChatId, e.UserId }, "UX_ChatUser_ChatId_UserId")
                .IsUnique()
                .HasFilter("(\"IsDeleted\" = false)");

            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_ChatUser_ChatId");

            entity.HasOne(d => d.Role).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatUser_RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ChatUser_UserId");
        });

        modelBuilder.Entity<ChatUserPermission>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.UserId, e.PermissionId }).HasName("PK_ChatUserPermission_Id");

            entity.ToTable("ChatUserPermission");

            entity.HasOne(d => d.Permission).WithMany(p => p.ChatUserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatUserPermission_PermissionId");

            entity.HasOne(d => d.ChatUser).WithMany(p => p.ChatUserPermissions)
                .HasForeignKey(d => new { d.ChatId, d.UserId })
                .HasConstraintName("FK_ChatUserPermission_ChatUser");
        });

        modelBuilder.Entity<Extension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Extension_Id");

            entity.ToTable("Extension");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<FileMetadataOperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileMetadataOperationStatus_Id");

            entity.ToTable("FileMetadataOperationStatus");

            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.TimeSet)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.FileMetadata).WithMany(p => p.FileMetadataOperationStatuses)
                .HasForeignKey(d => d.FileMetadataId)
                .HasConstraintName("FK_FileMetadataOperationStatus_MessageId");

            entity.HasOne(d => d.OperationStatus).WithMany(p => p.FileMetadataOperationStatuses)
                .HasForeignKey(d => d.OperationStatusId)
                .HasConstraintName("FK_FileMetadataOperationStatus_OperationStatusId");
        });

        modelBuilder.Entity<FileMetadatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileMetadata_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Extension).WithMany(p => p.FileMetadata)
                .HasForeignKey(d => d.ExtensionId)
                .HasConstraintName("PK_FileMetadata_ExtensionId");
        });

        modelBuilder.Entity<OperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OperationStatus_Id");

            entity.ToTable("OperationStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Permission_Id");

            entity.ToTable("Permission");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Role_Id");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "Role_Name_key").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK_RolePermission_Id");

            entity.ToTable("RolePermission");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_RolePermission_PermissionId");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RolePermission_RoleId");
        });

        modelBuilder.Entity<Sticker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sticker_Id");

            entity.ToTable("Sticker");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.File).WithMany(p => p.Stickers)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_Sticker_FileId");

            entity.HasOne(d => d.StickerPack).WithMany(p => p.Stickers)
                .HasForeignKey(d => d.StickerPackId)
                .HasConstraintName("FK_Sticker_StickerPackId");
        });

        modelBuilder.Entity<StickerPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StickerPack_Id");

            entity.ToTable("StickerPack");

            entity.HasIndex(e => e.AuthorId, "IX_StickerPack_AuthorId").HasFilter("(\"IsDeleted\" = false)");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Author).WithMany(p => p.StickerPacks)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_StickerPack_AuthorId");

            entity.HasOne(d => d.Preview).WithMany(p => p.StickerPacks)
                .HasForeignKey(d => d.PreviewId)
                .HasConstraintName("FK_StickerPack_PreviewId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.ForwardMessage).HasDefaultValue(true);
            entity.Property(e => e.InviteMe).HasDefaultValue(true);
            entity.Property(e => e.MonthsBeforeDelete).HasDefaultValue(6);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SearchMe).HasDefaultValue(true);
            entity.Property(e => e.ShowMe).HasDefaultValue(true);
            entity.Property(e => e.WasOnline)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<UserAvatar>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.FileId }).HasName("PK_UserAvatar_Id");

            entity.ToTable("UserAvatar");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.File).WithMany(p => p.UserAvatars)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_UserAvatar_FileId");

            entity.HasOne(d => d.User).WithMany(p => p.UserAvatars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserAvatar_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
