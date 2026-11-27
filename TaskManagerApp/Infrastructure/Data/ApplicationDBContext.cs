using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<TaskTag> TaskTags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskTag>()
                .HasIndex(tag => new { tag.UserId, tag.Name })
                .IsUnique();

            builder.Entity<TaskTag>()
                .Property(tag => tag.Color)
                .HasDefaultValue("#6258E8");

            builder.Entity<TaskItem>()
                .HasOne(task => task.Tag)
                .WithMany(tag => tag.Tasks)
                .HasForeignKey(task => task.TaskTagId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
