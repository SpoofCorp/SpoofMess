using Microsoft.EntityFrameworkCore;

namespace SpoofFileService.Models;

public partial class SpoofFileServiceContext : DbContext
{
    public SpoofFileServiceContext()
    {
    }

    public SpoofFileServiceContext(DbContextOptions<SpoofFileServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Extension> Extensions { get; set; }

    public virtual DbSet<FileObject> FileObjects { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category_Id");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Extension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Extension_Id");

            entity.ToTable("Extension");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<FileObject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileObject_Id");

            entity.ToTable("FileObject");

            entity.HasIndex(e => new { e.PreFingerprint, e.Size }, "IX_FileObject_Fast_Check");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Category).WithMany(p => p.FileObjects)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FileObject_CategoryId");

            entity.HasOne(d => d.Extension).WithMany(p => p.FileObjects)
                .HasForeignKey(d => d.ExtensionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FileObject_ExtensionId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
