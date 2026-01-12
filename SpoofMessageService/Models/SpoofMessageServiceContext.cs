using System;
using System.Collections.Generic;
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

    public virtual DbSet<Extension> Extensions { get; set; }

    public virtual DbSet<FileMetadatum> FileMetadata { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<ViewMessage> ViewMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, e.FileMetadataId }).HasName("PK_Attachment_Id");

            entity.ToTable("Attachment");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.FileMetadata).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.FileMetadataId)
                .HasConstraintName("FK_Attachment_FileMetadataId");

            entity.HasOne(d => d.Message).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_Attachment_MessageId");
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
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Text).HasDefaultValueSql("''::text");
        });

        modelBuilder.Entity<ViewMessage>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, e.UserId }).HasName("PK_ViewMessage_Id");

            entity.ToTable("ViewMessage");

            entity.HasIndex(e => e.MessageId, "IX_ViewMessage_MessageId");

            entity.HasIndex(e => e.UserId, "IX_ViewMessage_UserId");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ViewTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Message).WithMany(p => p.ViewMessages)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK_ViewMessage_MessageId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
