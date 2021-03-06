// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newspaper.Models;

namespace Newspaper.Data
{
    public partial class NewspaperdbContext : DbContext
    {
        public NewspaperdbContext()
        {
        }

        public NewspaperdbContext(DbContextOptions<NewspaperdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Newspaper;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Title).IsFixedLength();

                entity.HasOne(d => d.Category_NameNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.Category_Name)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_Category");

                entity.HasOne(d => d.Writer)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.WriterID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_Users");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.AdminID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.Username).HasDefaultValueSql("('Anonymous')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}