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

    public virtual DbSet<FileObject> FileObjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileObject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileObject_Id");

            entity.ToTable("FileObject");

            entity.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
