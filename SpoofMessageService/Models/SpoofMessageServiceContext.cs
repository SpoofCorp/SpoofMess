using Microsoft.EntityFrameworkCore;

namespace SpoofMessageService.Models;

public partial class SpoofMessageServiceContext : DbContext
{
    public SpoofMessageServiceContext()
    {
    }

    public SpoofMessageServiceContext(DbContextOptions<SpoofMessageServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; }

    public virtual DbSet<Extension> Extensions { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<FileMetadatum> FileMetadata { get; set; }

    public virtual DbSet<FileType> FileTypes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageOperationStatus> MessageOperationStatuses { get; set; }

    public virtual DbSet<OperationStatus> OperationStatuses { get; set; }

    public virtual DbSet<ViewMessage> ViewMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => new { e.Key1, e.Key2 }).HasName("PK_Attachment_Id");

            entity.ToTable("Attachment");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.FileMetadata).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.Key2)
                .HasConstraintName("FK_Attachment_FileMetadataId");

            entity.HasOne(d => d.Message).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.Key1)
                .HasConstraintName("FK_Attachment_MessageId");
        });

        modelBuilder.Entity<AttachmentOperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AttachmentOperationStatus_Id");

            entity.ToTable("AttachmentOperationStatus");

            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.TimeSet)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.OperationStatus).WithMany(p => p.AttachmentOperationStatuses)
                .HasForeignKey(d => d.OperationStatusId)
                .HasConstraintName("FK_AttachmentOperationStatus_OperationStatusId");

            entity.HasOne(d => d.Attachment).WithMany(p => p.AttachmentOperationStatuses)
                .HasForeignKey(d => new { d.MessageId, d.FileMetadataId })
                .HasConstraintName("FK_AttachmentOperationStatus_AttachmentId");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Chat_Id");

            entity.ToTable("Chat");

            entity.HasIndex(e => e.UniqueName, "Chat_UniqueName_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UniqueName).HasMaxLength(100);
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => new { e.Key1, e.Key2 }).HasName("PK_ChatUser_Id");

            entity.ToTable("ChatUser");

            entity.Property(e => e.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.Key1)
                .HasConstraintName("FK_ChatUser_ChatId");

            entity.HasOne(d => d.User).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.Key2)
                .HasConstraintName("FK_ChatUser_UserId");
        });

        modelBuilder.Entity<Extension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Extension_Id");

            entity.ToTable("Extension");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<FileMetadatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileMetadata_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Extension).WithMany(p => p.FileMetadata)
                .HasForeignKey(d => d.ExtensionId)
                .HasConstraintName("PK_FileMetadata_ExtensionId");
        });

        modelBuilder.Entity<FileType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileType_Id");

            entity.ToTable("FileType");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Message_Id");

            entity.ToTable("Message");

            entity.HasIndex(e => e.UserId, "IX_Message_AuthorId");

            entity.HasIndex(e => e.ChatId, "IX_Message_ChatId");

            entity.HasIndex(e => new { e.ChatId, e.SentAt }, "IX_Message_LastMessages");

            entity.HasIndex(e => e.SentAt, "IX_Message_SentAt");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Text).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_Message_ChatId");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Message_UserId");
        });

        modelBuilder.Entity<MessageOperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MessageOperationStatus_Id");

            entity.ToTable("MessageOperationStatus");

            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.TimeSet)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageOperationStatuses)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_MessageOperationStatus_MessageId");

            entity.HasOne(d => d.OperationStatus).WithMany(p => p.MessageOperationStatuses)
                .HasForeignKey(d => d.OperationStatusId)
                .HasConstraintName("FK_MessageOperationStatus_OperationStatusId");
        });

        modelBuilder.Entity<OperationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OperationStatus_Id");

            entity.ToTable("OperationStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "User_Login_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ViewMessage>(entity =>
        {
            entity.HasKey(e => new { e.Key2, e.Key1 }).HasName("PK_ViewMessage_Id");

            entity.ToTable("ViewMessage");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ViewTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Message).WithMany(p => p.ViewMessages)
                .HasForeignKey(d => d.Key2)
                .HasConstraintName("FK_ViewMessage_MessageId");

            entity.HasOne(d => d.User).WithMany(p => p.ViewMessages)
                .HasForeignKey(d => d.Key1)
                .HasConstraintName("FK_ViewMessage_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
