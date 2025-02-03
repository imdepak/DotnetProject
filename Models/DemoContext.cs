using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyDotnetProject.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Demo;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F729A45AC");

            entity.ToTable("employee");

            entity.HasIndex(e => e.EmpName, "UK_grvis7f915cwp5qf1c0bnrd9v").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmpName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("emp_name");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA477BF200239");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.LocationName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ParentLocationId).HasColumnName("ParentLocationID");

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.InverseParentLocation)
                .HasForeignKey(d => d.ParentLocationId)
                .HasConstraintName("FK__Locations__Paren__6FE99F9F");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__student__2A33069A3AE4F5E1");

            entity.ToTable("student");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.StudentEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("student_email");
            entity.Property(e => e.StudentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("student_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
